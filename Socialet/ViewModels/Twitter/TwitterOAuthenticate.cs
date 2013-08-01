using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Socialet.ViewModels.OAuthenticate;
using Windows.Security.Authentication.Web;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace Socialet.ViewModels.Twitter
{
    public class TwitterOAuth : OpenAuthenticate
    {
        private readonly ISendData oAuth;
        private const string consumerKey = "r1r5jqDS2KEbgvFhxS6dg";
        private const string ConsumerSecret = "1ufWtkJWibOUbGYDolKnlZr6HqxYmznUvAUpz6Hu3c";
        private static String requestTokenUrl = "https://api.twitter.com/oauth/request_token";
        private static String accessTokenUrl = "https://api.twitter.com/oauth/access_token";
        private string oauth_token;
        private string oauth_token_secret;
        private string accessToken;
        private const string SignatureMethod = "HMAC-SHA1";
        private const String twitterCallbackUrl = "https://192.168.1.1/oauth/callback";
        private const string AuthVersion = "1.0";

        public TwitterOAuth(ISendData oAuth)
        {
            this.oAuth = oAuth;
        }

        public override async Task<string> Authenticate()
        {
            try
            {
                var sigBaseStringParams = GetSigBaseStringParams();
                var sigBaseString = GetSigBaseString();

                var signature = GetAuthSignature(sigBaseString);


                requestTokenUrl += "?" + sigBaseStringParams + "&oauth_signature=" + Uri.EscapeDataString(signature);

                String getResponse = await oAuth.SendDataAsync(requestTokenUrl);

                if (getResponse != null)
                {
                    oauth_token = null;
                    oauth_token_secret = null;
                    String[] keyValPairs = getResponse.Split('&');

                    foreach (string parameter in keyValPairs)
                    {
                        String[] splits = parameter.Split('=');
                        switch (splits[0])
                        {
                            case "oauth_token":
                                oauth_token = splits[1];
                                break;
                            case "oauth_token_secret":
                                oauth_token_secret = splits[1];
                                break;
                        }
                    }

                    if (oauth_token != null)
                    {

                        requestTokenUrl = "https://api.twitter.com/oauth/authorize?oauth_token=" + oauth_token;
                        var startUri = new Uri(requestTokenUrl);
                        var endUri = new Uri(twitterCallbackUrl);

                        WebAuthenticationResult WebAuthenticationResult =
                            await WebAuthenticationBroker.AuthenticateAsync(
                                WebAuthenticationOptions.None,
                                startUri,
                                endUri);
                        if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                        {
                            string authVerifier = null;
                            String[] AccessTokenkeyValPairs = WebAuthenticationResult.ResponseData.Split('&');

                            foreach (string parameter in AccessTokenkeyValPairs)
                            {
                                String[] splits = parameter.Split('=');
                                switch (splits[0])
                                {
                                    case "oauth_verifier":
                                        authVerifier = splits[1];
                                        break;
                                }
                            }
                            var accessTokenResult = await GetAccessToken(oauth_token, authVerifier);
                            oauth_token = accessTokenResult.Dictionary["oauth_token_secret"];
                            accessToken = accessTokenResult.Dictionary["oauth_token_secret"];
                        }
                        else if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
                        {
                            // OutputToken("HTTP Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseErrorDetail.ToString());
                        }
                        else
                        {
                            //OutputToken("Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseStatus.ToString());
                        }
                    }
                }
            }
            catch (Exception Error)
            {
                //
                // Bad Parameter, SSL/TLS Errors and Network Unavailable errors are to be handled here.
                //
                //DebugPrint(Error.ToString());
            }

            return string.Empty;
        }

        public override void Post(string thisIsATestTweet)
        {
            //oauth_token = "66644379-Ig1ctJ9mMlMIcTdGkhbRfSPF0PAWeTu90wriHoIfJ";
            //oauth_token_secret = "EwiEa9jbFwx1BZuHp0HiL2bRFmlS46lFq1Q1LkfAaM";




            const string resourceUrl = "https://api.twitter.com/1.1/statuses/update.json";
            const string status = "dash Updating status via REST API if this works";

            const string baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                                      "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&status={6}";

            var oauthNonce = new Random().Next(Int32.MaxValue).ToString();
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var oauth_timestamp = Convert.ToInt64(ts.TotalSeconds).ToString();
            var baseString = string.Format(baseFormat,
                                        consumerKey,
                                        oauthNonce,
                                        SignatureMethod,
                                        oauth_timestamp,
                                        oauth_token,
                                        AuthVersion,
                                        Uri.EscapeDataString(status)
                                        );

            baseString = string.Concat("POST&", Uri.EscapeDataString(resourceUrl), "&", Uri.EscapeDataString(baseString));
            string signingKey = Uri.EscapeDataString(ConsumerSecret) + "&" + Uri.EscapeDataString(accessToken);
            IBuffer keyMaterial = CryptographicBuffer.ConvertStringToBinary(signingKey, BinaryStringEncoding.Utf8);

            MacAlgorithmProvider hmacSha1Provider = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
            CryptographicKey macKey = hmacSha1Provider.CreateKey(keyMaterial);
            IBuffer dataToBeSigned = CryptographicBuffer.ConvertStringToBinary(baseString, BinaryStringEncoding.Utf8);
            IBuffer signatureBuffer = CryptographicEngine.Sign(macKey, dataToBeSigned);
            String signature = CryptographicBuffer.EncodeToBase64String(signatureBuffer);

            //creating header
            const string headerFormat = "OAuth oauth_consumer_key=\"{0}\", oauth_nonce=\"{1}\", " +
                                        "oauth_signature=\"{2}\", oauth_signature_method=\"{3}\", " +
                                        "oauth_timestamp=\"{4}\", oauth_token=\"{5}\", " +
                                        "oauth_version=\"{6}\"";

            var authHeader = string.Format(headerFormat,
                                    Uri.EscapeDataString(consumerKey),
                                    Uri.EscapeDataString(oauthNonce),
                                    Uri.EscapeDataString(signature),
                                    Uri.EscapeDataString(SignatureMethod),
                                    Uri.EscapeDataString(oauth_timestamp),
                                    Uri.EscapeDataString(oauth_token),
                                    Uri.EscapeDataString(AuthVersion)
                            );

            var postBody = "status=" + Uri.EscapeDataString(status);
            var result = PostData(resourceUrl, authHeader, postBody);
            //var httpClient = new HttpClient();
            ////httpClient.BaseAddress = new Uri(resourceUrl);
            //httpClient.DefaultRequestHeaders.ExpectContinue = false;
            //httpClient.DefaultRequestHeaders.Add("Authorization", authHeader);

            //byte[] content = System.Text.Encoding.Unicode.GetBytes(postBody);

            //var byteArrayContent = new ByteArrayContent(content);
            //byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            //var response = httpClient.PostAsync(resourceUrl, byteArrayContent);
            //var httpResponseMessage = response.Result;
            //var isSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
            //var result = PostData(resourceUrl, postBody);

        }

        async Task<TwitterPostResults> PostData(String url, String headers, String requestData = null)
        {
            try
            {
                //var postBody = "screen_name=" + Uri.EscapeDataString("zish");//

                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(url);
                Request.Method = "POST";
                Request.Headers["Authorization"] = headers;
                Request.ContentType = "application/x-www-form-urlencoded";

                if (!String.IsNullOrEmpty(requestData))
                {
                    using (StreamWriter RequestDataStream = new StreamWriter(await Request.GetRequestStreamAsync()))
                    {
                        await RequestDataStream.WriteAsync(requestData);
                    }
                }

                HttpWebResponse Response = (HttpWebResponse)await Request.GetResponseAsync();

                if (Response.StatusCode != HttpStatusCode.OK)
                {
                    return new TwitterPostResults
                    {
                        Status = TwitterPostResults.EStatus.Error,
                        Description = Response.StatusDescription
                    };
                }

                using (StreamReader ResponseDataStream = new StreamReader(Response.GetResponseStream()))
                {
                    var response = await ResponseDataStream.ReadToEndAsync();
                    return new TwitterPostResults
                    {
                        Status = TwitterPostResults.EStatus.Success,
                        Dictionary = new TwitterDictionary(response)
                    };
                }
            }
            catch (Exception e)
            {
                return new TwitterPostResults
                {
                    Status = TwitterPostResults.EStatus.Error,
                    Description = e.Message,
                };
            }
        }


        public async Task<string> PostData(string url, string postData)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.MaxResponseContentBufferSize = int.MaxValue;
                httpClient.DefaultRequestHeaders.ExpectContinue = false;
                HttpRequestMessage requestMsg = new HttpRequestMessage();
                requestMsg.Content = new StringContent(postData);
                requestMsg.Method = new HttpMethod("POST");
                requestMsg.RequestUri = new Uri(url, UriKind.Absolute);
                requestMsg.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var response = await httpClient.SendAsync(requestMsg);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception Err)
            {
                throw;
            }
        }

        private string GetAuthSignature(string sigBaseString)
        {
            IBuffer keyMaterial = CryptographicBuffer.ConvertStringToBinary(ConsumerSecret + "&", BinaryStringEncoding.Utf8);
            MacAlgorithmProvider hmacSha1Provider = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
            CryptographicKey macKey = hmacSha1Provider.CreateKey(keyMaterial);
            IBuffer dataToBeSigned = CryptographicBuffer.ConvertStringToBinary(sigBaseString, BinaryStringEncoding.Utf8);
            IBuffer signatureBuffer = CryptographicEngine.Sign(macKey, dataToBeSigned);
            String signature = CryptographicBuffer.EncodeToBase64String(signatureBuffer);
            return signature;
        }

        private string GetSigBaseStringParams()
        {
            String sigBaseStringParams = "oauth_callback=" + Uri.EscapeDataString(twitterCallbackUrl);
            sigBaseStringParams += "&" + "oauth_consumer_key=" + consumerKey;
            sigBaseStringParams += "&" + "oauth_nonce=" + GetNonce();
            sigBaseStringParams += "&" + "oauth_signature_method=" + SignatureMethod.Replace('_', '-');
            sigBaseStringParams += "&" + "oauth_timestamp=" + Math.Round(GetSinceEpoch().TotalSeconds);
            sigBaseStringParams += "&" + "oauth_version=1.0";
            return sigBaseStringParams;
        }

        private string GetSigBaseString()
        {
            // Compute base signature string and sign it.
            //    This is a common operation that is required for all requests even after the token is obtained.
            //    Parameters need to be sorted in alphabetical order
            //    Keys and values should be URL Encoded.
            //

            var sigBaseString = "GET&";
            sigBaseString += Uri.EscapeDataString(requestTokenUrl) + "&" + Uri.EscapeDataString(GetSigBaseStringParams());
            return sigBaseString;
        }

        async Task<TwitterPostResults> GetAccessToken(String oauthToken, String oauthVerifier)
        {
            var header = new TwitterDictionary();
            header.Add("oauth_consumer_key", consumerKey);
            header.Add("oauth_nonce", GetNonce());
            header.Add("oauth_signature_method", SignatureMethod);
            header.Add("oauth_timestamp", Math.Round(GetSinceEpoch().TotalSeconds).ToString());
            header.Add("oauth_token", oauthToken);
            header.Add("oauth_version", AuthVersion);
            var request = new TwitterDictionary();
            request.Add("oauth_verifier", Uri.EscapeDataString(oauthVerifier));
            return await PostData(accessTokenUrl, header, request);  // should contain oauth_token, oauth_token_secret, user_id, and screen_name
        }

        async Task<TwitterPostResults> PostData(String url, TwitterDictionary headerDictionary, TwitterDictionary requestDictionary = null)
        {
            // See https://dev.twitter.com/docs/auth/creating-signature
            var combinedDictionaries = new TwitterDictionary(headerDictionary);
            combinedDictionaries.Add(requestDictionary);
            var signatureBase = "POST&" + Uri.EscapeDataString(url) + "&" + Uri.EscapeDataString(combinedDictionaries.ToStringA());
            var keyMaterial = CryptographicBuffer.ConvertStringToBinary(ConsumerSecret + "&" + oauth_token_secret, BinaryStringEncoding.Utf8);
            var algorithm = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
            var key = algorithm.CreateKey(keyMaterial);
            var dataToBeSigned = CryptographicBuffer.ConvertStringToBinary(signatureBase, BinaryStringEncoding.Utf8);
            var signatureBuffer = CryptographicEngine.Sign(key, dataToBeSigned);
            var signature = CryptographicBuffer.EncodeToBase64String(signatureBuffer);
            var headers = "OAuth " + headerDictionary.ToStringQ() + ", oauth_signature=\"" + Uri.EscapeDataString(signature) + "\"";
            return await PostData(url, headers, (requestDictionary == null) ? String.Empty : requestDictionary.ToString());
        }
    }
}
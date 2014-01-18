using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ClientLibrary.Authentication.OAuthenticate;
using ClientLibrary.Credentials;
using ClientLibrary.SocialMedia.Twitter;
using Windows.Security.Authentication.Web;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace ClientLibrary.Authentication
{
    public class TwitterOAuth : OpenAuthenticate
    {
        private const string ConsumerKey = "JHIoiNqHBS9WhPQYoYMc1w";
        private const string ConsumerSecret = "74SuQrAu3dzjpX4f1GIXwOM6APnw69fp4F4ddM0dyU";
        private const string SignatureMethod = "HMAC-SHA1";
        private const String TwitterCallbackUrl = "https://192.168.1.1/oauth/callback";
        private const string AuthVersion = "1.0";
        private static String requestTokenUrl = "https://api.twitter.com/oauth/request_token";
        private const String AccessTokenUrl = "https://api.twitter.com/oauth/access_token";
        private readonly ISendData oAuth;
        private string accessToken;
        private string oauthToken;
        private string oauthTokenSecret;

        public TwitterOAuth(ISendData oAuth)
        {
            this.oAuth = oAuth;
        }

        public override async Task<OpenAuthenticationResult> Authenticate()
        {
            try
            {
                var sigBaseStringParams = GetSigBaseStringParams();
                var sigBaseString = GetSigBaseString();
                var signature = GetAuthSignature(sigBaseString);
                
                requestTokenUrl += "?" + sigBaseStringParams + "&oauth_signature=" + Uri.EscapeDataString(signature);

                var getResponse = await oAuth.SendDataAsync(requestTokenUrl, false);

                if (getResponse != null)
                {
                    oauthToken = null;
                    oauthTokenSecret = null;
                    var keyValPairs = getResponse.Split('&');

                    foreach (var parameter in keyValPairs)
                    {
                        var splits = parameter.Split('=');
                        switch (splits[0])
                        {
                            case "oauth_token":
                                oauthToken = splits[1];
                                break;
                            case "oauth_token_secret":
                                oauthTokenSecret = splits[1];
                                break;
                        }
                    }

                    if (oauthToken != null)
                    {
                        requestTokenUrl = "https://api.twitter.com/oauth/authorize?oauth_token=" + oauthToken;
                        var startUri = new Uri(requestTokenUrl);
                        var endUri = new Uri(TwitterCallbackUrl);

                        var webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                                WebAuthenticationOptions.None,
                                startUri,
                                endUri);
                        if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                        {
                            string authVerifier = null;
                            var accessTokenkeyValPairs = webAuthenticationResult.ResponseData.Split('&');

                            foreach (string parameter in accessTokenkeyValPairs)
                            {
                                var splits = parameter.Split('=');
                                switch (splits[0])
                                {
                                    case "oauth_verifier":
                                        authVerifier = splits[1];
                                        break;
                                }
                            }
                            TwitterPostResults accessTokenResult = await GetAccessToken(oauthToken, authVerifier);
                            oauthToken = accessTokenResult.Dictionary["oauth_token"];
                            accessToken = accessTokenResult.Dictionary["oauth_token_secret"];
                            var credentials = new TwitterCredentials();
                            credentials.SaveCredential(oauthToken, accessToken);
                            return new OpenAuthenticationResult
                                {
                                    Credentials = credentials,
                                    WebAuthenticationResult = webAuthenticationResult.ResponseStatus
                                };
                        }
                        //else if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
                        {
                            return new OpenAuthenticationResult
                                {
                                    WebAuthenticationResult = webAuthenticationResult.ResponseStatus
                                };
                            // OutputToken("HTTP Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseErrorDetail.ToString());
                        }
                        //else
                        //{
                        //    //OutputToken("Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseStatus.ToString());
                        //}
                    }
                }
            }
            catch (Exception Error)
            {
                //
                // Bad Parameter, SSL/TLS Errors and Network Unavailable errors are to be handled here.
                //
                //DebugPrint(Error.ToString());
                return new OpenAuthenticationResult
                {
                   
                };
            }

            return null;
        }

        public override async Task<PostingResult> Post(string status, string resourceUrl)
        {
            //oauth_token = "66644379-Ig1ctJ9mMlMIcTdGkhbRfSPF0PAWeTu90wriHoIfJ";
            //oauth_token_secret = "EwiEa9jbFwx1BZuHp0HiL2bRFmlS46lFq1Q1LkfAaM";
            var credentials = new TwitterCredentials();
            var tokenAndSecret = credentials.GetCredential();
            oauthToken = tokenAndSecret.Item1;
            accessToken = tokenAndSecret.Item2;

            //const string resourceUrl = "https://api.twitter.com/1.1/statuses/update.json";

            const string baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                                      "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&status={6}";

            var oauthNonce = new Random().Next(Int32.MaxValue).ToString();
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var oauthTimestamp = Convert.ToInt64(ts.TotalSeconds).ToString();
            var baseString = string.Format(baseFormat,
                                              ConsumerKey,
                                              oauthNonce,
                                              SignatureMethod,
                                              oauthTimestamp,
                                              oauthToken,
                                              AuthVersion,
                                              Uri.EscapeDataString(status)
                );

            baseString = string.Concat("POST&", Uri.EscapeDataString(resourceUrl), "&", Uri.EscapeDataString(baseString));
            string signingKey = Uri.EscapeDataString(ConsumerSecret) + "&" + Uri.EscapeDataString(accessToken);
            var keyMaterial = CryptographicBuffer.ConvertStringToBinary(signingKey, BinaryStringEncoding.Utf8);

            var hmacSha1Provider = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
            var macKey = hmacSha1Provider.CreateKey(keyMaterial);
            var dataToBeSigned = CryptographicBuffer.ConvertStringToBinary(baseString, BinaryStringEncoding.Utf8);
            var signatureBuffer = CryptographicEngine.Sign(macKey, dataToBeSigned);
            var signature = CryptographicBuffer.EncodeToBase64String(signatureBuffer);

            //creating header
            const string headerFormat = "OAuth oauth_consumer_key=\"{0}\", oauth_nonce=\"{1}\", " +
                                        "oauth_signature=\"{2}\", oauth_signature_method=\"{3}\", " +
                                        "oauth_timestamp=\"{4}\", oauth_token=\"{5}\", " +
                                        "oauth_version=\"{6}\"";

            var authHeader = string.Format(headerFormat,
                                              Uri.EscapeDataString(ConsumerKey),
                                              Uri.EscapeDataString(oauthNonce),
                                              Uri.EscapeDataString(signature),
                                              Uri.EscapeDataString(SignatureMethod),
                                              Uri.EscapeDataString(oauthTimestamp),
                                              Uri.EscapeDataString(oauthToken),
                                              Uri.EscapeDataString(AuthVersion)
                );

            var postBody = "status=" + Uri.EscapeDataString(status);
            TwitterPostResults result = await PostData(resourceUrl, authHeader, postBody);
            
            return new PostingResult { Status = result.Status, Description = result.Description };
        }

        private async Task<TwitterPostResults> PostData(String url, String headers, String requestData = null)
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(url);
                request.Method = "POST";
                request.Headers["Authorization"] = headers;
                request.ContentType = "application/x-www-form-urlencoded";

                if (!String.IsNullOrEmpty(requestData))
                {
                    Stream stream = await request.GetRequestStreamAsync();
                    
                    using (var requestDataStream = new StreamWriter(stream))
                    {
                        await requestDataStream.WriteAsync(requestData);
                    }
                }

                using (var Response = (HttpWebResponse) await request.GetResponseAsync())
                {
                    if (Response.StatusCode != HttpStatusCode.OK)
                    {
                        return new TwitterPostResults
                            {
                                Status = PostingStatus.Error,
                                Description = Response.StatusDescription,
                                
                                
                            };
                    }

                    using (var ResponseDataStream = new StreamReader(Response.GetResponseStream()))
                    {
                        var response = await ResponseDataStream.ReadToEndAsync();
                        return new TwitterPostResults()
                            {
                                Status = PostingStatus.Success,
                                Description = Response.StatusDescription,
                                Dictionary = new TwitterDictionary(response)
                                
                            };
                    }
                }
            }
            catch (Exception e)
            {
                return new TwitterPostResults()
                    {
                        Status = PostingStatus.Error,
                        Description = e.Message,
                    };
            }
        }
        
        public async Task<string> PostData(string url, string postData)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.MaxResponseContentBufferSize = int.MaxValue;
                httpClient.DefaultRequestHeaders.ExpectContinue = false;
                var requestMsg = new HttpRequestMessage();
                requestMsg.Content = new StringContent(postData);
                requestMsg.Method = new HttpMethod("POST");
                requestMsg.RequestUri = new Uri(url, UriKind.Absolute);
                requestMsg.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                HttpResponseMessage response = await httpClient.SendAsync(requestMsg);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception Err)
            {
                throw;
            }
        }

        private string GetAuthSignature(string sigBaseString)
        {
            var keyMaterial = CryptographicBuffer.ConvertStringToBinary(ConsumerSecret + "&",
                                                                            BinaryStringEncoding.Utf8);
            var hmacSha1Provider = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
            var macKey = hmacSha1Provider.CreateKey(keyMaterial);
            var dataToBeSigned = CryptographicBuffer.ConvertStringToBinary(sigBaseString, BinaryStringEncoding.Utf8);
            var signatureBuffer = CryptographicEngine.Sign(macKey, dataToBeSigned);
            var signature = CryptographicBuffer.EncodeToBase64String(signatureBuffer);
            return signature;
        }

        private string GetSigBaseStringParams()
        {
            var sigBaseStringParams = "oauth_callback=" + Uri.EscapeDataString(TwitterCallbackUrl);
            sigBaseStringParams += "&" + "oauth_consumer_key=" + ConsumerKey;
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
            sigBaseString += Uri.EscapeDataString(requestTokenUrl) + "&" +
                             Uri.EscapeDataString(GetSigBaseStringParams());
            return sigBaseString;
        }

        private async Task<TwitterPostResults> GetAccessToken(String oauthToken, String oauthVerifier)
        {
            var header = new TwitterDictionary();
            header.Add("oauth_consumer_key", ConsumerKey);
            header.Add("oauth_nonce", GetNonce());
            header.Add("oauth_signature_method", SignatureMethod);
            header.Add("oauth_timestamp", Math.Round(GetSinceEpoch().TotalSeconds).ToString());
            header.Add("oauth_token", oauthToken);
            header.Add("oauth_version", AuthVersion);
            var request = new TwitterDictionary();
            request.Add("oauth_verifier", Uri.EscapeDataString(oauthVerifier));
            return await PostData(AccessTokenUrl, header, request); // should contain oauth_token, oauth_token_secret, user_id, and screen_name
        }

        private async Task<TwitterPostResults> PostData(String url, TwitterDictionary headerDictionary,
                                                        TwitterDictionary requestDictionary = null)
        {
            // See https://dev.twitter.com/docs/auth/creating-signature
            var combinedDictionaries = new TwitterDictionary(headerDictionary);
            combinedDictionaries.Add(requestDictionary);
            var signatureBase = "POST&" + Uri.EscapeDataString(url) + "&" +
                                   Uri.EscapeDataString(combinedDictionaries.ToStringA());
            var keyMaterial = CryptographicBuffer.ConvertStringToBinary(ConsumerSecret + "&" + oauthTokenSecret,
                                                                            BinaryStringEncoding.Utf8);
            var algorithm = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
            var key = algorithm.CreateKey(keyMaterial);
            var dataToBeSigned = CryptographicBuffer.ConvertStringToBinary(signatureBase, BinaryStringEncoding.Utf8);
            var signatureBuffer = CryptographicEngine.Sign(key, dataToBeSigned);
            var signature = CryptographicBuffer.EncodeToBase64String(signatureBuffer);
            var headers = "OAuth " + headerDictionary.ToStringQ() + ", oauth_signature=\"" +
                             Uri.EscapeDataString(signature) + "\"";
            return
                await PostData(url, headers, (requestDictionary == null) ? String.Empty : requestDictionary.ToString());
        }
    }

    public class OpenAuthenticationResult
    {
        public WebAuthenticationStatus WebAuthenticationResult { get; set; }
        public string ErrorDescription { get; set; }
        public IStoreCredentials Credentials { get; set; }
    }
}
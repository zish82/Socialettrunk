﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using ClientLibrary.Credentials;
using Windows.Security.Authentication.Web;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace ClientLibrary.SocialMedia.Twitter
{
    /// <summary>
    ///     Summary description for Tweet
    /// </summary>
    public class Twitter : BaseEntity, IAmSocialNetworkAndICanAuthenticate
    {
        const string StatusUpdateUrl = "https://api.twitter.com/1.1/statuses/update.json";
        const string UserTimeLineUrl = "https://api.twitter.com/1.1/statuses/user_timeline.json";
        private const string ConsumerKey = "JHIoiNqHBS9WhPQYoYMc1w";
        private const string ConsumerSecret = "74SuQrAu3dzjpX4f1GIXwOM6APnw69fp4F4ddM0dyU";
        private const string SignatureMethod = "HMAC-SHA1";
        private const string AuthVersion = "1.0";

        public Twitter(IOpenAuthenticate openAuthenticate)
        {
            OpenAuthenticate = openAuthenticate;
            AuthenticatedCredentials = new TwitterCredentials();

        }

        public IEnumerable<Tweet> Tweets { get; set; }
        public IOpenAuthenticate OpenAuthenticate { get; set; }
        public IStoreCredentials AuthenticatedCredentials { get; set; }
        public string Tweet { get; set; }
        public bool IsAuthenticated
        {
            get
            {
                return AuthenticatedCredentials.GetCredential() != null;
            }
        }
        
        public async Task<PostingResult> Post(string post)
        {
            return await OpenAuthenticate.Post(post, StatusUpdateUrl);
            //return new Task<PostingResult>(() => new PostingResult());
        }

        public void GetPosts()
        {
            var header = GetStatuses(UserTimeLineUrl);// GetHeader(UserTimeLineUrl);
        }

        public async void Authenticate()
        {
            var result = await OpenAuthenticate.Authenticate();
            if (result.WebAuthenticationResult == WebAuthenticationStatus.Success)
                AuthenticatedCredentials = result.Credentials;
        }

        public void ClearAuthentication()
        {
            AuthenticatedCredentials.RemoveCredential();
        }

        //public string GetHeader(string resourceUrl)
        //{
        //    var credentials = new TwitterCredentials();
        //    var tokenAndSecret = credentials.GetCredential();
        //    var oauthToken = tokenAndSecret.Item1;
        //    var accessToken = tokenAndSecret.Item2;
            
        //    var oauth_version = "1.0";
        //    var oauth_signature_method = "HMAC-SHA1";

        //    const string baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
        //                              "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}";// "&status={6}";

        //    var oauthNonce = new Random().Next(Int32.MaxValue).ToString();
        //    var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        //    var oauthTimestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();
        //    var baseString = string.Format(baseFormat,
        //                                      ConsumerKey,
        //                                      oauthNonce,
        //                                      SignatureMethod,
        //                                      oauthTimestamp,
        //                                      oauthToken,
        //                                      AuthVersion
        //                                      //,
        //                                      //Uri.EscapeDataString("")
        //        );

        //    baseString = string.Concat("GET&", Uri.EscapeDataString(resourceUrl), "&", Uri.EscapeDataString(baseString));
        //    string signingKey = Uri.EscapeDataString(ConsumerSecret) + "&" + Uri.EscapeDataString(accessToken);
        //    var keyMaterial = CryptographicBuffer.ConvertStringToBinary(signingKey, BinaryStringEncoding.Utf8);

        //    var hmacSha1Provider = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
        //    var macKey = hmacSha1Provider.CreateKey(keyMaterial);
        //    var dataToBeSigned = CryptographicBuffer.ConvertStringToBinary(baseString, BinaryStringEncoding.Utf8);
        //    var signatureBuffer = CryptographicEngine.Sign(macKey, dataToBeSigned);
        //    var signature = CryptographicBuffer.EncodeToBase64String(signatureBuffer);

        //    //creating header
        //    const string headerFormat = "OAuth oauth_consumer_key=\"{0}\", oauth_nonce=\"{1}\", " +
        //                                "oauth_signature=\"{2}\", oauth_signature_method=\"{3}\", " +
        //                                "oauth_timestamp=\"{4}\", oauth_token=\"{5}\", " +
        //                                "oauth_version=\"{6}\"";

        //    var authHeader = string.Format(headerFormat,
        //                                      Uri.EscapeDataString(ConsumerKey),
        //                                      Uri.EscapeDataString(oauthNonce),
        //                                      Uri.EscapeDataString(signature),
        //                                      Uri.EscapeDataString(SignatureMethod),
        //                                      Uri.EscapeDataString(oauthTimestamp),
        //                                      Uri.EscapeDataString(oauthToken),
        //                                      Uri.EscapeDataString(AuthVersion)
        //        );

        //    resourceUrl += "?" + authHeader
        //    return authHeader;
        //}
        
        public async Task<IList<RootObject>> GetStatuses(string resourceUrl)
        {
            resourceUrl = UserTimeLineUrl;
            var credentials = new TwitterCredentials();
            var tokenAndSecret = credentials.GetCredential();
            var oauthToken = tokenAndSecret.Item1;
            var accessToken = tokenAndSecret.Item2;
            
            const string baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                                      "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}";

            var oauthNonce = new Random().Next(Int32.MaxValue).ToString();
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var oauthTimestamp = Convert.ToInt64(ts.TotalSeconds).ToString();
            var baseString = string.Format(baseFormat,
                                              ConsumerKey,
                                              oauthNonce,
                                              SignatureMethod,
                                              oauthTimestamp,
                                              oauthToken,
                                              AuthVersion
                );

            baseString = string.Concat("GET&", Uri.EscapeDataString(resourceUrl), "&", Uri.EscapeDataString(baseString));
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

            //var postBody = "status=" + Uri.EscapeDataString(status);
            IList<RootObject> result = await PostData(resourceUrl, authHeader);

            return result;
        }

        private async Task<IList<RootObject>> PostData(String url, String headers, String requestData = null)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Headers["Authorization"] = headers;
                request.ContentType = "application/x-www-form-urlencoded";


                using (var response = (HttpWebResponse) await request.GetResponseAsync())
                {

                    using (var stream = (response.GetResponseStream()))
                    {
                        var jsonSerializer = new DataContractJsonSerializer(typeof(List<RootObject>));
                        //StreamReader reader = new StreamReader(stream);
                        //string tweets = reader.ReadToEnd();
                        var results = (List<RootObject>)jsonSerializer.ReadObject(stream);  
                        return  results;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
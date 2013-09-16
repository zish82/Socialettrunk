﻿using System;
using System.Threading.Tasks;
using ClientLibrary.Credentials;
using ClientLibrary.SocialMedia.Twitter;
using Facebook;
using Windows.Security.Authentication.Web;

namespace ClientLibrary.Authentication
{
    public class FacebookOAuth : OpenAuthenticate
    {
        private string FacebookAppID = "334764598835";
        private string appSecret = "b2bd2f23e54660b531304d870513aa97";
        private string FacebookCallbackUrl = "https://www.facebook.com/connect/login_success.html";

        internal static string AccessToken = String.Empty;
        internal static string FacebookId = String.Empty;
        public static bool isAuthenticated = false;
        public FacebookClient FacebookSessionClient;
        private string scope;


        public override async Task<OpenAuthenticationResult> Authenticate()
        {
            //FacebookSessionClient = new FacebookClient(FacebookAppID);

            if (FacebookAppID == "")
            {
                //rootPage.NotifyUser("Please enter an Client ID.", NotifyType.StatusMessage);
            }
            else if (FacebookCallbackUrl == "")
            {
                //rootPage.NotifyUser("Please enter an Callback URL.", NotifyType.StatusMessage);
            }

            try
            {
                scope = "manage_notifications,read_friendlists,read_stream";

                String FacebookURL = string.Format("https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&scope={2}{3}", 
                    Uri.EscapeDataString(FacebookAppID),
                    Uri.EscapeDataString(FacebookCallbackUrl),
                    scope,
                    "&display=popup&response_type=token");
                var startUri = new Uri(FacebookURL, UriKind.RelativeOrAbsolute);
                var endUri = new Uri(FacebookCallbackUrl, UriKind.RelativeOrAbsolute);


                var webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                                                        WebAuthenticationOptions.None,
                                                        startUri,
                                                        endUri);
                if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    string accessToken = null;
                    var accessTokenkeyValPairs = webAuthenticationResult.ResponseData.Split('&');

                    foreach (string parameter in accessTokenkeyValPairs)
                    {
                        var splits = parameter.Split('=');
                        if(splits[0].Contains("access_token"))
                        {
                                accessToken = splits[1];
                                var credentials = new FacebookCredentials();
                                credentials.SaveCredential(credentials.CredentialName, accessToken);
                                return new OpenAuthenticationResult()
                                {
                                    Credentials = credentials,
                                    WebAuthenticationResult = webAuthenticationResult.ResponseStatus
                                };
                        }
                    }

                    
                }
                else if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
                {
                    //OutputToken("HTTP Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseErrorDetail.ToString());
                }
                else
                {
                    //OutputToken("Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseStatus.ToString());
                }
            }
            catch (Exception Error)
            {
                // 
                // Bad Parameter, SSL/TLS Errors and Network Unavailable errors are to be handled here. 
                // 
                //DebugPrint(Error.ToString());
            }
            return null;
        }

        //remove this method from here, it does not look right here
        public override void Post(string thisIsATestTweet)
        {
            throw new NotImplementedException();
        }
    }
}
﻿using System;
using System.Threading.Tasks;
using ClientLibrary.SocialMedia.Twitter;
using Facebook;
using Windows.Security.Authentication.Web;

namespace ClientLibrary.Authentication
{
    public class FacebookOAuth : IOpenAuthenticate
    {
        private string FacebookAppID = "334764598835";
        private string appSecret = "b2bd2f23e54660b531304d870513aa97";
        private string FacebookCallbackUrl = @"http://textcreek.com";

        internal static string AccessToken = String.Empty;
        internal static string FacebookId = String.Empty;
        public static bool isAuthenticated = false;
        public FacebookClient FacebookSessionClient;
        

        public async Task<OpenAuthenticationResult> Authenticate()
        {
            //FacebookSessionClient client = new FacebookSessionClient();
            FacebookSessionClient = new FacebookClient(FacebookAppID);

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
                String FacebookURL = "https://www.facebook.com/dialog/oauth?client_id=" + 
                    Uri.EscapeDataString(FacebookAppID) + 
                    "&redirect_uri=" + Uri.EscapeDataString(FacebookCallbackUrl) + 
                    "&scope=read_stream&display=popup&response_type=token";

                System.Uri StartUri = new Uri(FacebookURL);
                System.Uri EndUri = new Uri(FacebookCallbackUrl);

                //DebugPrint("Navigating to: " + FacebookURL);

                var webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                                                        WebAuthenticationOptions.None,
                                                        StartUri,
                                                        EndUri);
                if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    //OutputToken(WebAuthenticationResult.ResponseData.ToString());
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
        public void Post(string thisIsATestTweet)
        {
            throw new NotImplementedException();
        }
    }
}
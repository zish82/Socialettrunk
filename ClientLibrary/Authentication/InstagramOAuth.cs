﻿using System;
using System.Threading.Tasks;
using ClientLibrary.SocialMedia.Twitter;
using Windows.Security.Authentication.Web;

namespace ClientLibrary.Authentication
{
    //public class InstagramOAuth : IOpenAuthenticate
    //{
    //    private string ClientID = "373edc327a1341dfa0f47df912490084";
    //    private string clientSecret = "6f58af754ab44a3489019875ee596d74";
    //    private string redirectUri = "https://192.168.0.1/socialet";

    //    internal static string AccessToken = String.Empty;
    //    internal static string InstagramId = String.Empty;
    //    public static bool isAuthenticated = false;

    //    public async Task<OpenAuthenticationResult> Authenticate()
    //    {
    //        //InstagramSessionClient client = new InstagramSessionClient();


    //        try
    //        {
    //            String InstagramURL = string.Format("https://api.instagram.com/oauth/authorize/?client_id={0}&redirect_uri={1}&response_type=code", 
    //                Uri.EscapeDataString(ClientID),
    //                Uri.EscapeDataString(redirectUri)
    //                );
    //            //,publish_actions,publish_stream,read_friendlist
    //            System.Uri StartUri = new Uri(InstagramURL);
    //            System.Uri EndUri = new Uri(redirectUri);

    //            //DebugPrint("Navigating to: " + InstagramURL);

    //            var webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
    //                                                    WebAuthenticationOptions.None,
    //                                                    StartUri,
    //                                                    EndUri);
    //            if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
    //            {
    //                string accessToken = null;
    //                var accessTokenkeyValPairs = webAuthenticationResult.ResponseData.Split('&');

    //                foreach (string parameter in accessTokenkeyValPairs)
    //                {
    //                    var splits = parameter.Split('=');
    //                    if(splits[0].Contains("access_token"))
    //                    {
    //                            //accessToken = splits[1];
    //                            //var credentials = new InstagramCredentials();
    //                            //credentials.SaveCredential(credentials.CredentialName, accessToken);
    //                            //return new OpenAuthenticationResult()
    //                            //{
    //                            //    Credentials = credentials,
    //                            //    WebAuthenticationResult = webAuthenticationResult.ResponseStatus
    //                            //};
    //                    }
    //                }

                    
    //            }
    //            else if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
    //            {
    //                //OutputToken("HTTP Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseErrorDetail.ToString());
    //            }
    //            else
    //            {
    //                //OutputToken("Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseStatus.ToString());
    //            }
    //        }
    //        catch (Exception Error)
    //        {
    //            // 
    //            // Bad Parameter, SSL/TLS Errors and Network Unavailable errors are to be handled here. 
    //            // 
    //            //DebugPrint(Error.ToString());
    //        }
    //        return null;
    //    }

    //    //remove this method from here, it does not look right here
    //    public PostingResult Post(string thisIsATestTweet)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
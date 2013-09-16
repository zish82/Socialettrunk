﻿using System;
using System.Threading.Tasks;
using ClientLibrary.Authentication;
using ClientLibrary.Credentials;
using ClientLibrary.SocialMedia;
using Windows.Security.Authentication.Web;

namespace Socialet.ViewModels
{
    public class FaceBookViewModel : AuthenticableSocialNetWorkViewModel
    {
        //private Twitter twitter;

        public FaceBookViewModel(IAmSocialNetworkAndICanAuthenticate socialNetwork)
            : base(socialNetwork)
        {
            Title = "facebuch";
            //twitter = new Twitter("my tweet");
        }
    }
}
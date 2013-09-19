﻿using ClientLibrary.SocialMedia;

namespace Socialet.ViewModels
{
    public class FaceBookViewModel : AuthenticableSocialNetWorkViewModel
    {
        //private Twitter twitter;

        public FaceBookViewModel(IAmSocialNetworkAndICanAuthenticate socialNetwork)
            : base(socialNetwork)
        {
            Title = "facebuch";
            SubTitle = IsAuthenticated ? "Authenticated" : "Authenticate";
        }
    }
}
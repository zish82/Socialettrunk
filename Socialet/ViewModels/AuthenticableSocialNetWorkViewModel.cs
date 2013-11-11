﻿using ClientLibrary.SocialMedia;

namespace Socialet.ViewModels
{
    public abstract class AuthenticableSocialNetWorkViewModel : SocialNetworkViewModel
    {
        public new readonly IAmSocialNetworkAndICanAuthenticate SocialNetwork;

        protected AuthenticableSocialNetWorkViewModel(IAmSocialNetworkAndICanAuthenticate socialNetwork) 
            :base(socialNetwork)
        {
            SocialNetwork = socialNetwork;
        }

        public virtual void Authenticate()
        {
            SocialNetwork.Authenticate();
        }

        public bool IsAuthenticated {
            get
            {
                return SocialNetwork.IsAuthenticated;
            }
        }


        public void ClearAuthentication()
        {
            SocialNetwork.ClearAuthentication();
        }
    }
}
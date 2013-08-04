﻿using ClientLibrary.SocialMedia;
using ClientLibrary.SocialMedia.Twitter;

namespace Socialet.ViewModels
{
    public class TwitterViewModel : AuthenticableSocialNetWorkViewModel
    {
        private readonly IOpenAuthenticate openAuthenticate;
        //private Twitter twitter;

        //public TwitterViewModel(IOpenAuthenticate openAuthenticate)
        //{
        //    this.openAuthenticate = openAuthenticate;
        //    Title = "Twitter";
        //    SubTitle = "Register your Twitter account for tweeeeeting!";
        //    //twitter = new Twitter("my tweet");
        //}

        public TwitterViewModel(IAmSocialNetworkAndICanAuthenticate socialNetwork) : base(socialNetwork)
        {
            Title = "Twitter";
            SubTitle = "Register your Twitter account for tweeeeeting!";
            //twitter = new Twitter("my tweet");
        }
    }
}

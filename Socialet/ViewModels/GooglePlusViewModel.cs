﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ClientLibrary.SocialMedia;

namespace Socialet.ViewModels
{
    public class GooglePlusViewModel : AuthenticableSocialNetWorkViewModel
    {
        //private Twitter twitter;

        public GooglePlusViewModel(IAmSocialNetworkAndICanAuthenticate socialNetwork)
            : base(socialNetwork)
        {
            Title = "google plus";
            //twitter = new Twitter("my tweet");
        }

        public override Task<IEnumerable<PostViewModel>> GetPosts()
        {
            throw new System.NotImplementedException();
        }
    }
}
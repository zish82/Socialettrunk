﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ClientLibrary.SocialMedia;

namespace Socialet.ViewModels
{
    public class InstagramViewModel : AuthenticableSocialNetWorkViewModel
    {
        //private Twitter twitter;

        public InstagramViewModel(IAmSocialNetworkAndICanAuthenticate socialNetwork)
            : base(socialNetwork)
        {
            Title = "instagram";
            //twitter = new Twitter("my tweet");
        }

        public override Task<IEnumerable<PostViewModel>> GetPosts()
        {
            throw new System.NotImplementedException();
        }
    }
}
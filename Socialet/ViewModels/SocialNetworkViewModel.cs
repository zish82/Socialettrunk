﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ClientLibrary.SocialMedia;

namespace Socialet.ViewModels
{
    public abstract class SocialNetworkViewModel : BaseViewModel
    {
        protected IAmSocialNetwork SocialNetwork;
        private string title;

        protected SocialNetworkViewModel(IAmSocialNetwork socialNetwork)
        {
            SocialNetwork = socialNetwork;
        }

        public Task<string> AuthenticationResult { get; set; }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public string SubTitle { get; set; }

        public abstract Task<IEnumerable<TweetViewModel>> GetTweets();
    }
}
﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ClientLibrary.Authentication;
using ClientLibrary.SocialMedia.GooglePlus;
using Microsoft.Practices.ObjectBuilder2;
using Socialet.Common;
using Socialet.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Socialet.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class PostPage
    {
        private readonly ObservableCollection<AuthenticableSocialNetWorkViewModel> socialNetworks = new ObservableCollection<AuthenticableSocialNetWorkViewModel>();
        public ObservableCollection<AuthenticableSocialNetWorkViewModel> SocialNetWorks {
            get { return socialNetworks; }
        }
        public PostPage()
        {
            
            InitializeComponent();
            
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            var authenticableSocialNetWorkViewModels = (ObservableCollection<AuthenticableSocialNetWorkViewModel>) navigationParameter;
            authenticableSocialNetWorkViewModels.ForEach(x => socialNetworks.Add(x));
            DefaultViewModel["Items"] = socialNetworks;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }
        
        private void BtnPost_OnClick(object sender, RoutedEventArgs e)
        {
            socialNetworks[0].SocialNetwork.Post(txtPost.Text);
        }
    }
}

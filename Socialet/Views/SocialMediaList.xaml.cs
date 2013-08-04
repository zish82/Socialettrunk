﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Socialet.ViewModels;
using Socialet.ViewModels.OAuthenticate;
using Socialet.ViewModels.Twitter;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace Socialet.Views
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class SocialMediaList : Socialet.Common.LayoutAwarePage
    {
        readonly Frame rootFrame = Window.Current.Content as Frame;

        public SocialMediaList()
        {
            socialNetworks.Add(new TwitterViewModel(new TwitterOAuth(new SendData())));
            //socialNetworks.Add(new FaceBookViewModel());
            this.InitializeComponent();
        }

        public SocialNetworkViewModel SelectedSocialNetwork { get; set; }
        private readonly ObservableCollection<SocialNetworkViewModel> socialNetworks = new ObservableCollection<SocialNetworkViewModel>();

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
            // TODO: Assign a bindable collection of items to this.DefaultViewModel["Items"]
            DefaultViewModel["Items"] = socialNetworks;
        }


        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            SelectedSocialNetwork = ((SocialNetworkViewModel)e.ClickedItem);
            SelectedSocialNetwork.Authenticate();
            //txtResult.Text = SelectedSocialNetwork.AuthenticationResult.ToString();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof (PostPage), "AllGroups");
        }
    }
}

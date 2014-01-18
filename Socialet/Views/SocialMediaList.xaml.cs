using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ClientLibrary.Authentication;
using ClientLibrary.Authentication.OAuthenticate;
using ClientLibrary.SocialMedia.Facebook;
using ClientLibrary.SocialMedia.Twitter;
using Microsoft.Practices.ObjectBuilder2;
using Socialet.ViewModels;
using Windows.UI.Popups;
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
            socialNetworks.Add(new TwitterViewModel(new Twitter(new TwitterOAuth(new SendData()))));
            var faceBookViewModel = new FaceBookViewModel(new Facebook(new FacebookOAuth()));
            socialNetworks.Add(faceBookViewModel);
            InitializeComponent();
        }

        public AuthenticableSocialNetWorkViewModel SelectedSocialNetwork { get; set; }
        private readonly ObservableCollection<AuthenticableSocialNetWorkViewModel> socialNetworks = new ObservableCollection<AuthenticableSocialNetWorkViewModel>();

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
            SelectedSocialNetwork = ((AuthenticableSocialNetWorkViewModel)e.ClickedItem);
            if (SelectedSocialNetwork.IsAuthenticated)
            {
                var dialog = new MessageDialog("Selected Social network is already authenticated!", "!Information");
                dialog.Commands.Add(new UICommand("Authenticate Again", command => SelectedSocialNetwork.Authenticate()));
                dialog.Commands.Add(new UICommand("Cancel"));
                dialog.ShowAsync();
                return;
            }
            SelectedSocialNetwork.Authenticate();
            //SelectedSocialNetwork.IsAuthenticated = true;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (!socialNetworks.Any(x => x.IsAuthenticated))
            {
                var dialog = new MessageDialog("There is no authenticated social network available!", "Information!");
                dialog.ShowAsync();
                return;
            }

            rootFrame.Navigate(typeof (PostPage), socialNetworks);
        }

        private void ButtonBase_OnClear(object sender, RoutedEventArgs e)
        {
            MessageDialog dialog;
            if (!socialNetworks.Any(x => x.IsAuthenticated))
            {
                dialog = new MessageDialog("There is no authenticated social network available!", "Information!");
                dialog.ShowAsync();
                return;
            }

            dialog = new MessageDialog("Are you sure you want to clear saved information!", "Confirmation");
            dialog.Commands.Add(new UICommand("Yes", command => socialNetworks.ForEach(x => x.ClearAuthentication())));
            dialog.Commands.Add(new UICommand("No"));
            dialog.ShowAsync();
        }
    }
}

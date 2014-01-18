using ClientLibrary.SocialMedia;

namespace Socialet.ViewModels
{
    public abstract class AuthenticableSocialNetWorkViewModel : SocialNetworkViewModel
    {
        public new readonly IAmSocialNetworkAndICanAuthenticate SocialNetwork;
        private bool isAuthenticated;

        protected AuthenticableSocialNetWorkViewModel(IAmSocialNetworkAndICanAuthenticate socialNetwork) 
            :base(socialNetwork)
        {
            SocialNetwork = socialNetwork;
            IsAuthenticated = socialNetwork.IsAuthenticated;
        }

        public virtual void Authenticate()
        {
            SocialNetwork.Authenticate();
            OnPropertyChanged("AuthenticationStatus");
        }

        public bool IsAuthenticated
        {
            get { return SocialNetwork.IsAuthenticated; }
            set
            {
                isAuthenticated = value; 
                OnPropertyChanged(); 
                OnPropertyChanged("AuthenticationStatus");
            }
        }

        public string AuthenticationStatus { get { return IsAuthenticated ? "Authenticated" : "Not authenticated"; } }


        public void ClearAuthentication()
        {
            SocialNetwork.ClearAuthentication();
            OnPropertyChanged("AuthenticationStatus");
        }
    }
}
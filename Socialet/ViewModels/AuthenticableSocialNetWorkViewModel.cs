using ClientLibrary.SocialMedia;

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

        public void Authenticate()
        {
            SocialNetwork.Authenticate();
        }

        public bool IsAuthenticated {
            get { return SocialNetwork.AuthenticatedCredentials != null; }
        }
    }
}
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
    }
}
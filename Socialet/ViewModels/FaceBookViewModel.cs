using ClientLibrary.SocialMedia;

namespace Socialet.ViewModels
{
    public class FaceBookViewModel : AuthenticableSocialNetWorkViewModel
    {
        //private Twitter twitter;

        public FaceBookViewModel(IAmSocialNetworkAndICanAuthenticate socialNetwork)
            : base(socialNetwork)
        {
            Title = "facebuch";
            //twitter = new Twitter("my tweet");
        }
    }
    
    public class InstagramViewModel : AuthenticableSocialNetWorkViewModel
    {
        //private Twitter twitter;

        public InstagramViewModel(IAmSocialNetworkAndICanAuthenticate socialNetwork)
            : base(socialNetwork)
        {
            Title = "instagram";
            //twitter = new Twitter("my tweet");
        }
    }
}
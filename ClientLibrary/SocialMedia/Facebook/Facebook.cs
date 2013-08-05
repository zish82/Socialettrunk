using ClientLibrary.Credentials;
using ClientLibrary.SocialMedia.Twitter;

namespace ClientLibrary.SocialMedia.Facebook
{
    public class Facebook : IAmSocialNetworkAndICanAuthenticate
    {
        private readonly IOpenAuthenticate facebookOAuth;

        public Facebook(IOpenAuthenticate facebookOAuth)
        {
            this.facebookOAuth = facebookOAuth;
        }

        public PostingResult Post(string post)
        {
            return new PostingResult();
        }

        public IOpenAuthenticate OpenAuthenticate { get; set; }
        public IStoreCredentials AuthenticatedCredentials { get; set; }
        
        public void Authenticate()
        {
            facebookOAuth.Authenticate();
        }
    }
}
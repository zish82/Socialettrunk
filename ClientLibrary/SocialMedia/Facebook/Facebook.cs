using ClientLibrary.Credentials;
using ClientLibrary.SocialMedia.Twitter;
using Facebook;

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
            var fbCredentials = facebookOAuth.Authenticate().Result;
            var client = new FacebookClient(fbCredentials.Credentials.GetCredential().Item2);
            dynamic result = client.PostTaskAsync("me/feed", "my first post via windows 8 app!");
            var lastMessageId = result.id;
        }
    }
}
using System.Threading.Tasks;
using ClientLibrary.SocialMedia.Twitter;

namespace ClientLibrary.SocialMedia
{
    public interface IAmSocialNetwork : ICanPost
    {
        //marker interface
    }

    public interface ICanPost
    {
        PostingResult Post();
    }

    public interface IAmSocialNetworkAndICanAuthenticate : IAmSocialNetwork
    {
        IOpenAuthenticate OpenAuthenticate { get; set; }
        Task<string> Authenticate();
    }
}
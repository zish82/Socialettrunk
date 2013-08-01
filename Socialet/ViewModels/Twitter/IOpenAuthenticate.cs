using System.Threading.Tasks;

namespace Socialet.ViewModels.Twitter
{
    public interface IOpenAuthenticate
    {
        Task<string> Authenticate();
        void Post(string thisIsATestTweet);
    }
}
using System;
using System.Threading.Tasks;

namespace Socialet.ViewModels.Twitter
{
    public abstract class OpenAuthenticate : IOpenAuthenticate
    {
        public abstract Task<string> Authenticate();
        public abstract void Post(string thisIsATestTweet);

        protected string GetNonce()
        {
            return new Random().Next(123400, 9999999).ToString();
        }

        protected TimeSpan GetSinceEpoch()
        {
            return DateTime.UtcNow - new DateTime(1970, 1, 1);
        }
    }
}
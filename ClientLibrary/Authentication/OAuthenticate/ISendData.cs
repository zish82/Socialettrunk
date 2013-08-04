using System.Threading.Tasks;

namespace ClientLibrary.Authentication.OAuthenticate
{
    public interface ISendData
    {
        Task<string> SendDataAsync(string url, bool expectContinue = true);
    }
}
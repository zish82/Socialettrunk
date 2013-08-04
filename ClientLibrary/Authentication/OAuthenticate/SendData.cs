using System.Net.Http;
using System.Threading.Tasks;

namespace ClientLibrary.Authentication.OAuthenticate
{
    public class SendData : ISendData
    {
        public virtual async Task<string> SendDataAsync(string url, bool expectContinue = true)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.ExpectContinue = expectContinue;
            return await httpClient.GetStringAsync(url);
        }
    }
}
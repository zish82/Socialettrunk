using System.Threading.Tasks;

namespace Socialet.ViewModels
{
    public class SocialNetworkViewModel : BaseViewModel
    {
        private string title;
        public Task<string> AuthenticationResult { get; set; }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public SocialNetworkViewModel()
        {

        }
    }
}
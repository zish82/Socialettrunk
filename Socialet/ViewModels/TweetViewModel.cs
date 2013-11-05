using System;
using Socialet.Common;

namespace Socialet.ViewModels
{
    public class TweetViewModel : BindableBase
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }

        public string UserName { get; set; }
        public string StatusNumber { get; set; }

        public string ProfileImageUrl { get; set; }
    }
}
using System;

namespace Socialet.ViewModels.Twitter
{
    public class TwitterPostResults
    {
        public enum EStatus
        {
            Success = 0,
            Canceled = 1,
            Error = 2,
        }

        public EStatus Status { get; set; }
        public String Description { get; set; }
        public TwitterDictionary Dictionary { get; set; }
    }
}
﻿using System;
using ClientLibrary.Authentication;

namespace ClientLibrary.SocialMedia.Twitter
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
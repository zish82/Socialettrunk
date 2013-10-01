﻿using System.Threading.Tasks;
using ClientLibrary.Authentication;

namespace ClientLibrary.SocialMedia.Twitter
{
    public interface IOpenAuthenticate
    {
        Task<OpenAuthenticationResult> Authenticate();
        Task<PostingResult> Post(string thisIsATestTweet);
    }
}
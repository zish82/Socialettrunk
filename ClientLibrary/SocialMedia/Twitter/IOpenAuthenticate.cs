﻿using System.Threading.Tasks;

namespace ClientLibrary.SocialMedia.Twitter
{
    public interface IOpenAuthenticate
    {
        Task<string> Authenticate();
    }
}
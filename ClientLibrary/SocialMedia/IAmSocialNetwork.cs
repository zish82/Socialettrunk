﻿using System.Threading.Tasks;
using ClientLibrary.Credentials;
using ClientLibrary.SocialMedia.Twitter;

namespace ClientLibrary.SocialMedia
{
    public interface IAmSocialNetwork : ICanPost
    {
        //marker interface
    }

    public interface ICanPost
    {
        PostingResult Post(string post);
    }

    public interface IAmSocialNetworkAndICanAuthenticate : IAmSocialNetwork
    {
        IOpenAuthenticate OpenAuthenticate { get; set; }
        IStoreCredentials AuthenticatedCredentials { get; set; }
        void Authenticate();
    }
}
﻿using System.Threading.Tasks;
using ClientLibrary.SocialMedia.Twitter;

namespace ClientLibrary.SocialMedia
{
    public interface IAmSocialNetwork : ICanPost
    {
        //marker interface
    }

    public interface ICanPost
    {
        Task<PostingResult> Post(string post);
    }
}
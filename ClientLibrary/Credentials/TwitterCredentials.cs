﻿using System;

namespace ClientLibrary.Credentials
{
    public class TwitterCredentials : StoreCredentials
    {
        public string AuthToken { get; set; }
        public const string AuthTokenKey = "AuthTokenKey";
        public string AccessToken { get; set; }
        public const string AccessTokenKey = "AccessTokenKey";

        public TwitterCredentials()
        {
            CredentialName = "TwitterCredential";
        }
    }
}
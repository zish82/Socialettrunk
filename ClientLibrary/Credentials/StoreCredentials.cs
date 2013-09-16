﻿using System;
using System.Linq;
using Windows.Security.Credentials;

namespace ClientLibrary.Credentials
{
    //todo zeeshan: write tests
    public abstract class StoreCredentials : IStoreCredentials
    {
        ////using Windows.Security.Credentials;
        //private string credentialName { get; set; }
        public string CredentialName { get; set; }

        public void SaveCredential(string userName, string password)
        {
            RemoveCredential();

            var vault = new PasswordVault();
            var credential = new PasswordCredential(CredentialName, userName, password);

            // Permanently stores credential in the password vault.
            vault.Add(credential);
        }

        public Tuple<string, string> GetCredential()
        {

            string userName = string.Empty, password = string.Empty;

            var vault = new PasswordVault();
            var credential = vault.FindAllByResource(CredentialName).FirstOrDefault();
            if (credential == null)
                return null;

            {
                // Retrieves the actual userName and password.
                userName = credential.UserName;
                password = vault.Retrieve(CredentialName, userName).Password;
            }

            return new Tuple<string, string>(userName, password);

        }

        public void RemoveCredential()
        {
            var vault = new PasswordVault();
            try
            {
                var credential = vault.FindAllByResource(CredentialName).FirstOrDefault();
                // Removes the credential from the password vault.
                vault.Remove(vault.Retrieve(CredentialName, credential.UserName));
            }
            catch (Exception)
            {
                // If no credentials have been stored with the given RESOURCE_NAME, an exception
                // is thrown.
            }
        }
    }
}
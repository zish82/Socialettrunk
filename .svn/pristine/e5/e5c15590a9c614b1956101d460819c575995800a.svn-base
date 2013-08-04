using System;

namespace ClientLibrary.Credentials
{
    public interface IStoreCredentials
    {
        void SaveCredential(string userName, string password);
        Tuple<string, string> GetCredential();
        void RemoveCredential(string userName);

    }
}
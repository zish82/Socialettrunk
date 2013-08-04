namespace ClientLibrary.Credentials
{
    public class TwitterCredentials : StoreCredentials
    {
        protected override string CredentialName { get; set; }

        public TwitterCredentials()
        {
            CredentialName = "TwitterCredential";
        }
    }
}
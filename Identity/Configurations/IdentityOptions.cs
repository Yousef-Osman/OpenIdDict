namespace Identity.Configurations
{
    public class IdentityOptions
    {
        public const string Key = "Identity";

        public const string ConnectionStringKey = "IdentityServerConnectionString";

        public string Authority { get; set; }

        public string UseDeveloperSigningCredential { get; set; }

        public string SigningCredentialThumbprint { get; set; }
    }
}

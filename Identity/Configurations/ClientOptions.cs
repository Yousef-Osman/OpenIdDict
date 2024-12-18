namespace Identity.Configurations;

public class ClientOptions
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Secret { get; set; }
    public int TokenLifetime { get; set; }
    public List<string> AllowedGrantTypes { get; set; }
    public bool RequirePkce { get; set; }
    public bool RequireClientSecret { get; set; }
    public List<string> RedirectUris { get; set; }
    public List<string> PostLogoutRedirectUris { get; set; }
    public List<string> AllowedScopes { get; set; }
}

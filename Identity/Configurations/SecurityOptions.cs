namespace Identity.Configurations;

public class SecurityOptions
{
    public const string Key = "Security";

    public string AllowedOrigins { get; set; }

    public string HstsMaxAgeInDays { get; set; }
}

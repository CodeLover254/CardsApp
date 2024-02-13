namespace CardsApp.Domain.Settings;

public class JwtSettings
{
    public const string Name = "Jwt";
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int TokenExpirySeconds { get; set; }
}
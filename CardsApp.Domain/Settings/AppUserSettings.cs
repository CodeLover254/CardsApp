namespace CardsApp.Domain.Settings;

public class AppUserSettings
{
    public const string Name = "AppUsers";
    public AppUser Admin { get; set; }
    public AppUser Member { get; set; }
}

public class AppUser
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
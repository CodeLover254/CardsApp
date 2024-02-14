namespace CardsApp.Domain.Dto.Auth;

public class UserLoginResponse
{
    public string AccessToken { get; set; }
    public int ExpiresIn { get; set; }
}
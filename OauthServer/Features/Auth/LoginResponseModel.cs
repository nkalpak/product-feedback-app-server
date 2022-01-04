using System.ComponentModel.DataAnnotations;

namespace OauthServer.Features.Auth;

public class LoginResponseModel
{
    [Required]
    public string AccessToken { get; set; }
}
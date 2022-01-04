using System.ComponentModel.DataAnnotations;

namespace OauthServer.Features.Auth;

public class RegisterResponseModel
{
    [Required]
    public string AccessToken { get; set; }
}
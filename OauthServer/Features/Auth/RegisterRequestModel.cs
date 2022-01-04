using System.ComponentModel.DataAnnotations;

namespace OauthServer.Features.Auth;

public class RegisterRequestModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}
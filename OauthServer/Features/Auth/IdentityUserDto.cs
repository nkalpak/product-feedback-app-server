using System.ComponentModel.DataAnnotations;

namespace OauthServer.Features.Auth;

public class IdentityUserDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Username { get; set; }
}
using Microsoft.AspNetCore.Identity;

namespace OauthServer.Features.Auth;

public class User : IdentityUser
{
    public string ProfilePictureUrl { get; set; }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace OauthServer.Features.Auth;

public interface IAuthService
{
    string GenerateToken(IdentityUser user);
}

public class AuthService : IAuthService
{
    public string GenerateToken(IdentityUser user)
    {
        var token = new JwtSecurityToken(
            Constants.Issuer,
            Constants.Audience,
            new[] {new Claim(JwtRegisteredClaimNames.Sub, user.Id)},
            DateTime.Now,
            DateTime.Now.AddDays(7),
            new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Secret)),
                SecurityAlgorithms.HmacSha256
            )
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
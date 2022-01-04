using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace OauthServer.Features.Auth;

[ApiController]
[Route("/api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;

    public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponseModel>> Register(RegisterRequestModel request)
    {
        var newUser = new IdentityUser {UserName = request.Username};
        var createResponse = await _userManager.CreateAsync(newUser, request.Password);
        if (!createResponse.Succeeded) return BadRequest(createResponse.Errors);

        var storedUser = await _userManager.FindByNameAsync(request.Username);

        var token = new JwtSecurityToken(
            Constants.Issuer,
            Constants.Audience,
            new[] {new Claim(JwtRegisteredClaimNames.Sub, storedUser.Id)},
            DateTime.Now,
            DateTime.Now.AddHours(1),
            new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Secret)),
                SecurityAlgorithms.HmacSha256
            )
        );

        return Ok(new RegisterResponseModel {AccessToken = token.ToString()});
    }
}
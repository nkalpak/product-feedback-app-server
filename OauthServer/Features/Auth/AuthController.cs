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
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel request)
    {
        var user = _userManager.FindByNameAsync(request.Username);
        if (user.Result == null) return BadRequest("Invalid username or password.");

        var signInResponse = await _signInManager.PasswordSignInAsync(user.Result, request.Password, false, false);
        if (!signInResponse.Succeeded) return BadRequest("Invalid username or password");

        var token = new JwtSecurityToken(
            Constants.Issuer,
            Constants.Audience,
            new[] {new Claim(JwtRegisteredClaimNames.Sub, user.Result.Id)},
            DateTime.Now,
            DateTime.Now.AddHours(1),
            new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Secret)),
                SecurityAlgorithms.HmacSha256
            )
        );
        var tokenHandler = new JwtSecurityTokenHandler();

        return Ok(new LoginResponseModel {AccessToken = tokenHandler.WriteToken(token)});
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

        var tokenHandler = new JwtSecurityTokenHandler();

        return Ok(new RegisterResponseModel {AccessToken = tokenHandler.WriteToken(token)});
    }
}
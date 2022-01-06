using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace OauthServer.Features.Auth;

[ApiController]
[Route("/api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IAuthService _authService;

    public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
        IAuthService authService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel request)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null) return BadRequest("Invalid username or password.");

        var signInResponse = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
        if (!signInResponse.Succeeded) return BadRequest("Invalid username or password");

        return Ok(new LoginResponseModel {AccessToken = _authService.GenerateToken(user)});
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponseModel>> Register(RegisterRequestModel request)
    {
        var newUser = new IdentityUser {UserName = request.Username};
        var createResponse = await _userManager.CreateAsync(newUser, request.Password);
        if (!createResponse.Succeeded) return BadRequest(createResponse.Errors);

        var storedUser = await _userManager.FindByNameAsync(request.Username);

        return Ok(new RegisterResponseModel {AccessToken = _authService.GenerateToken(storedUser)});
    }
}
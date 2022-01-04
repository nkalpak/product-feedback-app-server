using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace OauthServer.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet("Secret")]
    [Authorize]
    public IActionResult Secret()
    {
        return Ok("John");
    }

    [HttpGet("Authenticate")]
    public ActionResult Authenticate()
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "some_id"),
            new Claim("granny", "cookie")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Secret));

        var token = new JwtSecurityToken(
            Constants.Issuer,
            Constants.Audience,
            claims,
            DateTime.Now,
            DateTime.Now.AddHours(1),
            new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new {access_token = tokenString});
    }

    [HttpGet("Decode")]
    public IActionResult Decode(string part)
    {
        var bytes = Convert.FromBase64String(part);
        
        return Ok(Encoding.UTF8.GetString(bytes));
    }
}
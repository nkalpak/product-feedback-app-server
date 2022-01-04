using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OauthServer;

var builder = WebApplication.CreateBuilder(args);

{
    builder.Services.AddControllers();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowEverything", policyBuilder =>
        {
            policyBuilder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        });
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddOpenApiDocument();

    builder.Services
        .AddAuthentication("OAuth")
        .AddJwtBearer("OAuth", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Secret)),
                ValidIssuer = Constants.Issuer,
                ValidAudience = Constants.Audience
            };
        });

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("PfaDatabase"));
    });

    builder.Services
        .AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}

app.UseCors("AllowEverything");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
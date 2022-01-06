using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OauthServer;
using OauthServer.Features.Auth;
using OauthServer.Helpers;

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

    builder.Services.AddScoped<IAuthService, AuthService>();

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

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
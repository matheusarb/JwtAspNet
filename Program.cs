using System.Security.Claims;
using System.Text;
using JwtAspNet.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.WebEncoders.Testing;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<TokenService>();

builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.PrivateKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("admin", p => p.RequireRole("admin"));
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/login", (TokenService service)
    =>
    {
        var user = new User(
            1,
            "Matheus Ribeiro",
            "email@gmail.com",
            "pass123",
            "image",
            new[] { "student", "admin" }
        );

        return service.Create(user);
    });

app.MapGet("/restrito", (ClaimsPrincipal user) => new
{
    // id = user.Claims.FirstOrDefault(x => x.Type == "id").ToString(),
    // name = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).ToString(),
    // email = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).ToString(),
    // givenName = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName).ToString(),
    // image = user.Claims.FirstOrDefault(x => x.Type == "image").ToString()
    id = 1,
    name = "s",
    email = "s",
    givenName = "s",
    image = "image",
    role = new[]{"admin"}
})
    .RequireAuthorization("admin");

app.MapGet("/admin", () => "Acesso autorizado. Você é um admin")
    .RequireAuthorization("admin");

app.Run();

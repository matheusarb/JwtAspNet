using JwtAspNet.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<TokenService>();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", (TokenService service)
    =>
    {
        var user = new User(
            1,
            "Matheus Ribeiro",
            "email@gmail.com",
            "pass123",
            "image",
            new[] {
                "student",
                "premium"
            }
        );

        return service.Create(user);
    });

app.Run();

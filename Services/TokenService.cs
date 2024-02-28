using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtAspNet.Models;
using Microsoft.IdentityModel.Tokens;

public class TokenService
{
    public string Create(User user)
    {
        var handler = new JwtSecurityTokenHandler();

        //transformando a privateKey em array de bytes 
        var key = Encoding.ASCII.GetBytes(Configuration.PrivateKey);

        // assinar as credenciais
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256
        );

        //criar TokenDescriptor
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddHours(2),
            Subject = GenerateClaims(user)
        };
        
        var token = handler.CreateToken(tokenDescriptor);

        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(User user)
    {
        var claimsIdentity = new ClaimsIdentity();
        
        claimsIdentity.AddClaim(new Claim("Id", user.Id.ToString()));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
        claimsIdentity.AddClaim(new Claim("Image", user.Image));
        
        foreach(var role in user.Roles)
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));

        return claimsIdentity;
    }
}
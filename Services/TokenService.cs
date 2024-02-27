using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class TokenService
{
    public string Create()
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
            Expires = DateTime.UtcNow.AddHours(2)
        };
        
        var token = handler.CreateToken(tokenDescriptor);

        return handler.WriteToken(token);
    }
}
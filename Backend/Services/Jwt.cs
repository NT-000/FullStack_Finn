namespace Finn_klone.Backend.Services;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class Jwt
{
    private readonly string _secureKey;
    
    public Jwt(IConfiguration config)
    {
        _secureKey = config["JwtSettings:SecretKey"];
    }

    public string GenerateToken(int userId, string email)
    {
        var claims = new[]
        {
            new Claim("id", userId.ToString()),
            new Claim("email", email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secureKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "finn-klone",
            audience: "finn-klone-users",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
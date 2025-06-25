using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebSocket.dto;
using WebSocket.Entity;

public class TokenService
{
    private readonly string _secretKey = "yourSecretKeyyourSecretKeyyourSecretKeyyourSecretKey";
    private readonly string _issuer = "yourIssuer";
    private readonly string _audience = "yourAudience";

    public string GenerateToken(UserTokenDto user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Email),
            
        }; 
        claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name)));
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
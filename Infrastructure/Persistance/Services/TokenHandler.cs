using Application.DTOs;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Domain.Entities;

public class TokenHandler : ITokenHandler
{
    private readonly JwtSettings _jwtSettings;

    public TokenHandler(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public Token CreateAccessToken(AppUser user, string role)
    {
        var token = new Token();
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Role, role ?? string.Empty),
            new Claim("DepartmentId", user.DepartmentId ?? string.Empty)
        };

        token.Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes);
        JwtSecurityToken securityToken = new JwtSecurityToken(
            audience: _jwtSettings.Audience,
            issuer: _jwtSettings.Issuer,
            expires: token.Expiration,
            notBefore: DateTime.UtcNow,
            claims: claims,
            signingCredentials: signingCredentials
        );
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        token.AccessToken = jwtSecurityTokenHandler.WriteToken(securityToken);
        return token;
    }
}
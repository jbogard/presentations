#region Example 3
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IntegrationTests;

public class TestJwt
{
    public List<Claim> Claims { get; } = new();
    public int ExpiresInMinutes { get; set; } = 30;
    
    public string Build()
    {
        var token = new JwtSecurityToken(
            JwtProvider.Issuer,
            JwtProvider.Issuer,
            Claims,
            expires: DateTime.Now.AddMinutes(ExpiresInMinutes),
            signingCredentials: JwtProvider.SigningCredentials
        );
        return JwtProvider.JwtSecurityTokenHandler.WriteToken(token);
    }
}
#endregion
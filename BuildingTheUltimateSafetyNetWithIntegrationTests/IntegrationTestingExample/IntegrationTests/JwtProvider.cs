using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace IntegrationTests;

public static class JwtProvider
{
    public static string Issuer { get; } = "Sample_Auth_Server";
    public static SecurityKey SecurityKey { get; } =
        new SymmetricSecurityKey("secret_key_for_integration_tests"u8.ToArray());
    public static SigningCredentials SigningCredentials { get; } =
        new(SecurityKey, SecurityAlgorithms.HmacSha256);
    internal static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new();
}
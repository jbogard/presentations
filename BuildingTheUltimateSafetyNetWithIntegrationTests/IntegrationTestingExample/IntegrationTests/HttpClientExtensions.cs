#region Example 3
using System.Net.Http.Headers;
using System.Security.Claims;

namespace IntegrationTests;

public static class HttpClientExtensions
{
    public static HttpClient WithJwtBearerToken(
        this HttpClient client,
        Action<TestJwt>? configure = null
    )
    {
        var token = new TestJwt();
        token.Claims.Add(new Claim("sub", Guid.NewGuid().ToString()));
        configure?.Invoke(token);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            token.Build()
        );

        return client;
    }
}
#endregion
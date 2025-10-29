#region Example 3
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity.Data;

namespace WebApp.Endpoints;

public class UserLoginEndpoint : Endpoint<LoginRequest>
{
    public override void Configure()
    {
        Post("/api/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        if (req is { Email: "jimmy@email.com", Password: "P@ssw0rd" })
        {
            var jwtToken = JwtBearer.CreateToken(
                o =>
                {
                    o.SigningKey = "A super super super secret token signing key";
                    o.ExpireAt = DateTime.UtcNow.AddDays(1);
                    o.User.Roles.Add("Manager", "Auditor");
                    o.User.Claims.Add(("UserName", req.Email));
                    o.User["UserId"] = "001"; //indexer based claim setting
                });

            await Send.OkAsync(jwtToken, ct);
        }
        else
            ThrowError("The supplied credentials are invalid!");
    }
}
#endregion
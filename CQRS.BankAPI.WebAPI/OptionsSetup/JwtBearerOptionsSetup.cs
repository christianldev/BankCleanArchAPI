using System.Text;
using System.Text.Json;
using CQRS.BankAPI.Application.Wrappers;
using CQRS.BankAPI.Persistence.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CQRS.BankAPI.WebAPI.OptionsSetup;
public class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtOptions _jwtOptions;

    public JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        ConfigureToken(options);
    }

    public void Configure(JwtBearerOptions options)
    {
        ConfigureToken(options);
    }

    private void ConfigureToken(JwtBearerOptions options)
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = false;
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            IssuerSigningKey =
              new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey!))
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = c =>
            {
                c.NoResult();
                c.Response.StatusCode = 500;
                c.Response.ContentType = "text/plain";
                return c.Response.WriteAsync(c.Exception.ToString());
            },
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var res = JsonSerializer.Serialize(new Response<string>("Access denied. You are not authorized to access the resource"));
                return context.Response.WriteAsync(res);
            },
            OnForbidden = context =>
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                var res = JsonSerializer.Serialize(new Response<string>("You do not have permissions to access the resource"));
                return context.Response.WriteAsync(res);

            }
        };
    }

}
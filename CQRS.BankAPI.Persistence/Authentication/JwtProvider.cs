using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CQRS.BankAPI.Application.Authentication;
using CQRS.BankAPI.Application.Abstractions.Data;
using CQRS.BankAPI.Domain.Users;
using Dapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CQRS.BankAPI.Persistence.Authentication;
public sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public JwtProvider(
        IOptions<JwtOptions> options,
        ISqlConnectionFactory sqlConnectionFactory
        )
    {
        _options = options.Value;
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<string> Generate(User user)
    {
        const string sql = """
              SELECT 
                p.Nombre
              FROM Users usr
                LEFT JOIN Users_Roles usrl
                    ON usr.Id=usrl.UserId
                LEFT JOIN Roles rl
                    ON rl.Id=usrl.RoleId
                LEFT JOIN Roles_Permissions rp
                    ON rl.Id=rp.RoleId
                LEFT JOIN Permissions p
                    ON p.Id=rp.PermissionId
                WHERE usr.Id=@UserId            
        """;

        using var connection = _sqlConnectionFactory.CreateConnection();
        var permissions =
          await connection.QueryAsync<string>(sql, new { UserId = user.Id!.Value });

        var permissionCollection = permissions.ToHashSet();

        var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id!.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!.Value)
        };

        foreach (var permission in permissionCollection)
        {
            claims.Add(new(CustomClaims.Permissions, permission));
        }


        var sigingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey!)),
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            DateTime.UtcNow.AddDays(365),
            sigingCredentials
        );

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);


        return tokenValue;
    }
}
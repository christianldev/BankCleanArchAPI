using CQRS.BankAPI.Persistence.Authentication;
using Microsoft.Extensions.Options;

namespace CQRS.BankAPI.WebAPI.OptionsSetup;
public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
{
    private const string SectionName = "JwtSettings";
    private readonly IConfiguration _configuration;

    public JwtOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(JwtOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}
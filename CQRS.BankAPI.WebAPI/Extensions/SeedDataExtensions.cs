using Bogus;
using CQRS.BankAPI.Application.Helpers;
using CQRS.BankAPI.Domain;
using CQRS.BankAPI.Domain.Entities.Users;
using CQRS.BankAPI.Domain.Users;
using CQRS.BankAPI.Persistence.Contexts;
using Dapper;

namespace CQRS.BankAPI.WebAPI.Extensions;
public static class SeedDataExtensions
{

    public static void SeedDataAuthentication(
        this IApplicationBuilder app
    )
    {

        using var scope = app.ApplicationServices.CreateScope();
        var service = scope.ServiceProvider;
        var loggerFactory = service.GetRequiredService<ILoggerFactory>();

        try
        {
            var context = service.GetRequiredService<AppBankDbContext>();

            if (!context.Set<User>().Any())
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword("Test123$");

                var user = User.Create(
                    "Christian",
                    "Lopez",
                    "0932320658",
                    new Address("Calle Falsa 123", "Springfield", "Springfield"),
                    new PhoneNumber("0999999999"),
                    "clopez@gmail.com",
                    new PasswordHash(passwordHash),
                    new IpUser(IpHelper.GetIpAddress()),
                    UserStatus.Active
                );

                context.Add(user);

                passwordHash = BCrypt.Net.BCrypt.HashPassword("Admin123$");

                user = User.Create(
                    "Admin",
                    "Admin",
                    "0932320658",
                    new Address("Calle Falsa 123", "Springfield", "Springfield"),
                    new PhoneNumber("0999999999"),
                    "admin@gmail.com",
                    new PasswordHash(passwordHash),
                    new IpUser(IpHelper.GetIpAddress()),
                    UserStatus.Active
                );

                context.Add(user);

                context.SaveChangesAsync().Wait();

            }

        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<AppBankDbContext>();
            logger.LogError(ex.Message);
        }



    }

}
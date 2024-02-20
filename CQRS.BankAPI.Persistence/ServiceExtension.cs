using CQRS.BankAPI.Application.Abstractions.Data;
using CQRS.BankAPI.Application.Authentication;
using CQRS.BankAPI.Application.Interfaces;
using CQRS.BankAPI.Domain.Abstractions;
using CQRS.BankAPI.Domain.Entities.Users;
using CQRS.BankAPI.Persistence.Authentication;
using CQRS.BankAPI.Persistence.Contexts;
using CQRS.BankAPI.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CQRS.BankAPI.Persistence.Data;
using MediatR;
using CQRS.BankAPI.Application.Features.Authenticate.Command.AuthenticateCommand;
using CQRS.BankAPI.Application.DTOS.Response;
using CQRS.BankAPI.Application.Features.Users.Commands.AuthenticateUser;

namespace CQRS.BankAPI.Persistence
{
    public static class ServiceExtension
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
             ?? throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<AppBankDbContext>(options =>
            options.UseSqlServer(connectionString,
            b => b.MigrationsAssembly(typeof(AppBankDbContext).Assembly.FullName)));

            #region Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRequestHandler<LoginCommand, Result<TokenResponse>>, LoginCommandHandler>();
            services.AddScoped<IRequestHandler<GetUserByTokenCommand, Result<UserResponse>>, GetUserByTokenCommandHandler>();
            services.AddTransient(typeof(IRepositoryAsync<>), typeof(MyRepositoryAsync<>));
            services.AddTransient<IJwtProvider, JwtProvider>();
            services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppBankDbContext>());

            #endregion

            #region Caching

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetSection("Caching:RedisConnection").Get<string>();


            });
            #endregion

        }
    }
}

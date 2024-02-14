using CQRS.BankAPI.Application.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Net.NetworkInformation;
using System.Reflection;

namespace CQRS.BankAPI.Application
{
    public static class ServiceExtension
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyReference>();
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });
            services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyReference>();
            services.AddValidatorsFromAssembly(typeof(ServiceExtension).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


        }
    }
}

using CQRS.BankAPI.Application;
using CQRS.BankAPI.Application.Enums;
using CQRS.BankAPI.Identity;
using CQRS.BankAPI.Persistence;
using CQRS.BankAPI.Persistence.Authentication;
using CQRS.BankAPI.Shared;
using CQRS.BankAPI.WebAPI.Extensions;
using CQRS.BankAPI.WebAPI.OptionsSetup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Serilog;

namespace CQRS.BankAPI.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowMyApp",
                    builder => builder.WithOrigins("https://localhost:7238/"));
            });

            #region Services Config
            // Add services to the container.

            //Add Application Layer
            services.AddApplicationLayer();
            services.AddIdentityInfrastructure(Configuration);
            services.AddSharedInfrastructure(Configuration);
            services.AddPersistenceInfrastructure(Configuration);
            services.AddControllers();
            services.ConfigureOptions<JwtOptionsSetup>();
            services.AddApiVersioningExtension();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "CQrS Bank API", Version = "v1.0" });
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {securityScheme, new string[] { }}
        });

            });
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Admin", p => p.RequireRole(RolesEnum.Administrator.ToString()));
                auth.AddPolicy("Basic", p => p.RequireRole(RolesEnum.Basic.ToString()));
            });
            services
            .AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services
            .AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

            services.AddEndpointsApiExplorer();
            #endregion


        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseCors("AllowMyApp");
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    c.RoutePrefix = "swagger";
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseRequestContextLogging();
            app.UseSerilogRequestLogging();
            app.UseCustomExceptionHandler();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseErrorHandlingMiddleware();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            //app.MapControllers();

            //app.Run();


        }
    }
}

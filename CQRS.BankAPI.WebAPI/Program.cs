using Serilog;
using CQRS.BankAPI.Application;
using CQRS.BankAPI.Application.Enums;
using CQRS.BankAPI.Identity;
using CQRS.BankAPI.Persistence;
using CQRS.BankAPI.Persistence.Authentication;
using CQRS.BankAPI.Shared;
using CQRS.BankAPI.WebAPI.Extensions;
using CQRS.BankAPI.WebAPI.OptionsSetup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using Microsoft.OpenApi.Models;

// {
//     public class Program
//     {


//         public async static Task Main(string[] args)
//         {
//             var host = CreateHostBuilder(args).Build();

//             using var scope = host.Services.CreateScope();
//             {
//                 var services = scope.ServiceProvider;
//                 try
//                 {
//                     var tManager = services.GetRequiredService<RoleManager<IdentityRole>>();
//                     var uManager = services.GetRequiredService<UserManager<ApplicationUser>>();
//                     await DefaultRole.SeedAsync(uManager, tManager);
//                     await DefaultAdminUser.SeedAsync(uManager, tManager);
//                     await DefaultBasicUser.SeedAsync(uManager, tManager);
//                 }
//                 catch (Exception e)
//                 {

//                     throw;
//                 }
//             }
//             await host.RunAsync();
//         }
//         public static IHostBuilder CreateHostBuilder(string[] args) =>
//             Host.CreateDefaultBuilder(args)
//         .UseSerilog((hostingContext, loggerConfiguration) =>
//             loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration))
//         .ConfigureWebHostDefaults(webBuilder =>
//         {
//             webBuilder.UseStartup<Startup>();
//         });

//     }
// }




var builder = WebApplication.CreateBuilder(args);

#region Services Config

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyApp",
        builder => builder.WithOrigins("https://localhost:7238/"));
});

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();
// Add services to the container.


// builder.Services.AddDefaultIdentity<IdentityUser>()
//     .AddRoles<IdentityRole>() // This line adds the RoleManager service
//     .AddEntityFrameworkStores<IdentityContext>();
//Add Application Layer
builder.Services.AddApplicationLayer();
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddAuthentication(
   options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(
   options =>
 {
     options.RequireHttpsMetadata = false;
     options.SaveToken = true;
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuerSigningKey = true,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("SecretKey")),
         ValidateIssuer = false,
         ValidateAudience = false,
         ValidateLifetime = true,
         ClockSkew = TimeSpan.Zero
     };
 });
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddApiVersioningExtension();
builder.Services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Admin", p => p.RequireRole(RolesEnum.Administrator.ToString()));
                auth.AddPolicy("Basic", p => p.RequireRole(RolesEnum.Basic.ToString()));
            });
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CQrS Bank API", Version = "v1.0" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services
.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

builder.Services
.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

builder.Services.AddEndpointsApiExplorer();
#endregion

#region Middlewares Config
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseCors(
        options => options.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
    );
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseRouting();
await app.ApplyMigration();
app.SeedDataAuthentication();
app.UseRequestContextLogging();
app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();
app.UseErrorHandlingMiddleware();
app.UseCustomExceptionHandler();
app.MapControllers();

app.Run();
#endregion
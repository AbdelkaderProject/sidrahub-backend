using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SidraHub.Application.Common.Interfaces;
using SidraHub.Application.Services.Auth;
using SidraHub.Infrastructure.Identity;
using SidraHub.Infrastructure.Persistence;
using SidraHub.Infrastructure.Services;
using SidraHub.Infrastructure.Services;

namespace SidraHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection was not configured.");
        var issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt issuer was not configured.");
        var audience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt audience was not configured.");
        var key = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt key was not configured.");

        services.AddHttpContextAccessor();
        services.AddDbContext<SidraHubDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
                sqlOptions.EnableRetryOnFailure()));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<SidraHubDbContext>());
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.Configure<AdminSeedOptions>(configuration.GetSection("AdminSeed"));
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IdentityDataSeeder>();

        // Email Service
        services.Configure<EmailOptions>(configuration.GetSection("Email"));
        services.AddScoped<IEmailService, EmailService>();

        // Notification Service
        services.AddScoped<INotificationService, NotificationService>();

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<SidraHubDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });

        services.AddAuthorization();

        return services;
    }
}

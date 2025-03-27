using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MzadPalestine.Core.Interfaces;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Settings;
using MzadPalestine.Infrastructure.Data;
using MzadPalestine.Infrastructure.Repositories;
using MzadPalestine.Infrastructure.Services.BackgroundJobs;
using MzadPalestine.Infrastructure.Services.Cache;
using MzadPalestine.Infrastructure.Services.CurrentUser;
using MzadPalestine.Infrastructure.Services.Email;
using MzadPalestine.Infrastructure.Services.FileStorage;
using MzadPalestine.Infrastructure.Services.Identity;
using MzadPalestine.Infrastructure.Services.Notification;
using MzadPalestine.Infrastructure.Services.Payment;
using System.Text;

namespace MzadPalestine.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // Configure JWT Authentication
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
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
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                ClockSkew = TimeSpan.Zero
            };
        });

        // Configure Redis Cache
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "MzadPalestine:";
        });

        // Configure Hangfire
        services.AddHangfire(config =>
        {
            config.UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"));
        });
        services.AddHangfireServer();

        // Register Settings
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.Configure<FileStorageSettings>(configuration.GetSection("FileStorageSettings"));
        services.Configure<PaymentSettings>(configuration.GetSection("PaymentSettings"));
        services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));

        // Register Services
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IFileStorageService, LocalFileStorageService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICacheService, RedisCacheService>();
        services.AddScoped<IBackgroundJobService, HangfireBackgroundJobService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<JwtTokenService>();

        // Add SignalR
        services.AddSignalR();

        return services;
    }
}

using System.Text;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MzadPalestine.API.Middleware;
using MzadPalestine.Core.Configuration;
using MzadPalestine.Core.Interfaces;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models;
using MzadPalestine.Infrastructure.Data;
using MzadPalestine.Infrastructure.Repositories;
using MzadPalestine.Infrastructure.Services;
using MzadPalestine.Infrastructure.Services.Identity;
using MzadPalestine.Infrastructure.Services.Email;
using MzadPalestine.Infrastructure.Services.FileStorage;
using MzadPalestine.Infrastructure.Services.Payment;
using MzadPalestine.Infrastructure.Services.Notification;
using MzadPalestine.Infrastructure.Services.CurrentUser;
using MzadPalestine.Infrastructure.SignalR;
using MzadPalestine.Infrastructure;

namespace MzadPalestine.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure HTTPS
        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.ListenLocalhost(7222, listenOptions =>
            {
                listenOptions.UseHttps(httpsOptions =>
                {                    
                    httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13;
                });
            });
        });

        // Configure Services
        ConfigureServices(builder);

        var app = builder.Build();

        // Configure HTTP Pipeline
        ConfigurePipeline(app);

        // Initialize Database
        await InitializeDatabase(app);

        await app.RunAsync();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        // Add Infrastructure Services
        services.AddInfrastructure(configuration);

        // Core Services
        services.AddScoped<ITokenService , TokenService>();
        services.AddScoped<IPhotoService , PhotoService>();
        services.AddScoped<IUnitOfWork , UnitOfWork>();

        // Business Services
        services.AddScoped<IAuctionService , AuctionService>();
        services.AddScoped<IBidService , BidService>();
        services.AddScoped<Core.Interfaces.SignalR.ISignalRConnectionManager , Infrastructure.SignalR.UserConnectionManager>();
        services.AddScoped<Core.Interfaces.Services.IUserConnectionManager , Infrastructure.Services.UserConnectionManager>();

        // API and Documentation
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        ConfigureSwagger(services);

        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ,
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3 ,
                        maxRetryDelay: TimeSpan.FromSeconds(5) ,
                        errorNumbersToAdd: null);
                }));

        // Identity and Authentication
        ConfigureIdentity(services);
        ConfigureAuthentication(services , configuration);

        // CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin" ,
                builder => builder
                    .WithOrigins(configuration["AllowedOrigins"]!.Split(","))
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });

        // Application Services
        ConfigureApplicationServices(services , configuration);

        // Infrastructure
        services.AddSignalR();

        // Caching
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "MzadPalestine:";
        });

        // Background Jobs
        services.AddHangfire(config =>
            config.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));
        services.AddHangfireServer();

        // Exception Handling
        services.AddTransient<GlobalExceptionHandler>();
    }

    private static void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1" , new OpenApiInfo { Title = "MzadPalestine API" , Version = "v1" });
            c.AddSecurityDefinition("Bearer" , new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme" ,
                Name = "Authorization" ,
                In = ParameterLocation.Header ,
                Type = SecuritySchemeType.ApiKey ,
                Scheme = "Bearer"
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
    }

    private static void ConfigureIdentity(IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser , IdentityRole>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;

            // User settings
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
    }

    private static void ConfigureAuthentication(IServiceCollection services , IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true ,
                ValidateAudience = true ,
                ValidateLifetime = true ,
                ValidateIssuerSigningKey = true ,
                ValidIssuer = configuration["JwtSettings:Issuer"] ,
                ValidAudience = configuration["JwtSettings:Audience"] ,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]!)) ,
                ClockSkew = TimeSpan.Zero
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });
    }

    private static void ConfigureApplicationServices(IServiceCollection services , IConfiguration configuration)
    {
        // Add Infrastructure Services
        services.AddInfrastructure(configuration);

        // Core Services
        services.AddScoped<ITokenService , TokenService>();
        services.AddScoped<IPhotoService , PhotoService>();
        services.AddScoped<IUnitOfWork , UnitOfWork>();

        // Business Services
        services.AddScoped<IAuctionService , AuctionService>();
        services.AddScoped<IBidService , BidService>();
        services.AddScoped<Core.Interfaces.SignalR.ISignalRConnectionManager , Infrastructure.SignalR.UserConnectionManager>();
        services.AddScoped<Core.Interfaces.Services.IUserConnectionManager , Infrastructure.Services.UserConnectionManager>();

        // Add AutoMapper
        services.AddAutoMapper(typeof(Program).Assembly , typeof(ITokenService).Assembly);

        // Repositories
        services.AddScoped<Core.Interfaces.Repositories.IAuctionRepository , AuctionRepository>();
        services.AddScoped<Core.Interfaces.Repositories.IListingRepository , ListingRepository>();
        services.AddScoped<ICategoryRepository , CategoryRepository>();
        services.AddScoped<ILocationRepository , LocationRepository>();
        services.AddScoped<IWalletRepository , WalletRepository>();
        services.AddScoped<IMessageRepository , MessageRepository>();
        services.AddScoped<INotificationRepository , NotificationRepository>();
        services.AddScoped<IReviewRepository , ReviewRepository>();
    }

    private static void ConfigurePipeline(WebApplication app)
    {
        // Development specific middleware
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Global exception handling
        app.UseMiddleware<GlobalExceptionHandler>();

        // Security and routing
        app.UseHttpsRedirection();
        app.UseCors("AllowSpecificOrigin");
        app.UseAuthentication();
        app.UseAuthorization();

        // Endpoints
        app.MapControllers();
        app.MapHub<NotificationHub>("/hubs/notifications");

        // Hangfire Dashboard
        app.UseHangfireDashboard("/hangfire" , new DashboardOptions
        {
            Authorization = new[] { new HangfireAuthorizationFilter() }
        });
    }

    private static async Task InitializeDatabase(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.MigrateAsync();
            await Seed.SeedData(context , userManager , roleManager);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex , "An error occurred while migrating or seeding the database.");
        }
    }
}

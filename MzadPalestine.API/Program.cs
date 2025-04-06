using System.Text;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MzadPalestine.API.Filters;
using MzadPalestine.API.Middleware;
using MzadPalestine.Core.Configuration;
using MzadPalestine.Core.Interfaces.Repositories;  // 
using MzadPalestine.Core.Models;
using MzadPalestine.Infrastructure;
using MzadPalestine.Infrastructure.Data;
using MzadPalestine.Infrastructure.Repositories;  // 
using MzadPalestine.Infrastructure.Services;
using MzadPalestine.Infrastructure.Services.Email;
using MzadPalestine.Infrastructure.Services.FileStorage;
using MzadPalestine.Infrastructure.Services.Identity;
using MzadPalestine.Infrastructure.Services.Notification;
using MzadPalestine.Infrastructure.Services.Payment;
using MzadPalestine.Infrastructure.Services.CurrentUser;
using MzadPalestine.Infrastructure.SignalR;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Interfaces;
using System.Reflection;
namespace MzadPalestine.API;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureKestrel(builder.WebHost);

        ConfigureServices(builder);

        var app = builder.Build();

        ConfigurePipeline(app);

        await InitializeDatabase(app);

        await app.RunAsync();
    }

    private static void ConfigureKestrel(IWebHostBuilder webHostBuilder)
    {
        webHostBuilder.ConfigureKestrel(serverOptions =>
        {
            // منفذ HTTP
            serverOptions.ListenLocalhost(5000);
            
            // منفذ HTTPS
            serverOptions.ListenLocalhost(5001, listenOptions =>
            {
                listenOptions.UseHttps(httpsOptions =>
                {
                    httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 |
                                               System.Security.Authentication.SslProtocols.Tls13;
                });
            });
        });
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var config = builder.Configuration;

        services.AddControllers(options =>
        {
            options.Conventions.Add(new ApiControllerConvention());
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        });

        services.AddEndpointsApiExplorer();

        ConfigureSwagger(services);
        ConfigureDatabase(services, config);
        
        ConfigureCors(services, config);

        services.AddSignalR();

        services.AddAutoMapper(typeof(Program).Assembly);

        services.AddTransient<GlobalExceptionHandler>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = config.GetConnectionString("Redis");
            options.InstanceName = "MzadPalestine:";
        });

        services.AddInfrastructure(config);

        // SignalR Services
        services.AddSingleton<IUserConnectionManager, MzadPalestine.Infrastructure.SignalR.UserConnectionManager>();

        // Core Services
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Business Services
        services.AddScoped<IAuctionService, AuctionService>();
        services.AddScoped<IBidService, BidService>();

        // Repositories -   
        services.AddScoped<MzadPalestine.Core.Interfaces.Repositories.IAuctionRepository, MzadPalestine.Infrastructure.Repositories.AuctionRepository>();
        services.AddScoped<MzadPalestine.Core.Interfaces.Repositories.IListingRepository, MzadPalestine.Infrastructure.Repositories.ListingRepository>();
        services.AddScoped<MzadPalestine.Core.Interfaces.Repositories.INotificationRepository, MzadPalestine.Infrastructure.Repositories.NotificationRepository>();
        services.AddScoped<MzadPalestine.Core.Interfaces.Repositories.IReviewRepository, MzadPalestine.Infrastructure.Repositories.ReviewRepository>();
        services.AddScoped<MzadPalestine.Core.Interfaces.ISubscriptionRepository, MzadPalestine.Infrastructure.Repositories.SubscriptionRepository>();
        services.AddScoped<MzadPalestine.Core.Interfaces.ITagRepository, MzadPalestine.Infrastructure.Repositories.TagRepository>();
        services.AddScoped<MzadPalestine.Core.Interfaces.IWalletRepository, MzadPalestine.Infrastructure.Repositories.WalletRepository>();

        //// SignalR Connections - 
    }

    private static void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Mzad Palestine API",
                Version = "v1",
                Description = "RESTful API for Mzad Palestine auction platform",
                Contact = new OpenApiContact
                {
                    Name = "Mzad Palestine Team",
                    Email = "support@mzadpalestine.com",
                }
            });

            // إضافة فلتر لتجاهل خصائص الملفات
            options.SchemaFilter<IgnoreFormFileSchemaFilter>();

            // تعريف مخطط بسيط لملفات IFormFile
            options.MapType<IFormFile>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "binary"
            });

            // تعريف مخطط مصفوفة لمجموعة الملفات IFormFileCollection
            options.MapType<IFormFileCollection>(() => new OpenApiSchema
            {
                Type = "array",
                Items = new OpenApiSchema
                {
                    Type = "string",
                    Format = "binary"
                }
            });

            // تعريف مخطط Stream
            options.MapType<Stream>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "binary"
            });

            // استخدام معرفات أبسط للمخططات
            options.CustomSchemaIds(type => type.Name);

            // إعداد أمان JWT
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            // إضافة تعليقات XML إن وجدت
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }

            // تعطيل التحقق من صحة المخططات المعقدة
            options.UseInlineDefinitionsForEnums();
            options.UseOneOfForPolymorphism();
        });
    }

    private static void ConfigureDatabase(IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
    }

    private static void ConfigureCors(IServiceCollection services, IConfiguration config)
    {
        var origins = config["AllowedOrigins"]?.Split(',') ?? new[] { "*" };
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", policy =>
            {
                policy.WithOrigins(origins)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });
    }

    private static void ConfigurePipeline(WebApplication app)
    {
        // تمكين عرض الأخطاء التفصيلية
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        
        // تفعيل Swagger في جميع البيئات
        app.UseSwagger(c =>
        {
            c.SerializeAsV2 = false; // استخدام OpenAPI 3.0
            c.RouteTemplate = "swagger/{documentName}/swagger.json";
        });

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "MzadPalestine API v1");
            c.RoutePrefix = "swagger";
            c.DefaultModelsExpandDepth(-1);
            c.DocumentTitle = "Mzad Palestine API Documentation";
            c.EnableDeepLinking();
            c.DisplayRequestDuration();
            c.EnableValidator();
            c.EnableFilter();
            c.EnableTryItOutByDefault();
        });

        app.UseMiddleware<GlobalExceptionHandler>();
        app.UseHttpsRedirection();
        app.UseCors("AllowSpecificOrigin");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapHub<NotificationHub>("/hubs/notifications");
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
            await Seed.SeedData(context, userManager, roleManager);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Database migration or seeding failed.");
        }
    }
}

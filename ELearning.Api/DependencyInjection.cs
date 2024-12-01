using ELearning.Core.Base.ApiResponse;
using ELearning.Core.MediatrHandlers.Student.Queries.GetAllStudents;
using ELearning.Data;
using ELearning.Data.Consts;
using ELearning.Data.Settings;
using ELearning.Service.Service;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Stripe;
using System.Reflection;
using System.Threading.RateLimiting;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Caching.Hybrid;


namespace ELearning.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

               #pragma warning disable 
          services.AddHybridCache();
               #pragma warning restore

            // Configure database connection
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));


            //Health Checks Configure
            services.AddHealthChecks()
                       .AddSqlServer(name: "database", connectionString: connectionString!)
                       .AddHangfire(options => { options.MinimumAvailableServers = 1; });


            //Dependencies of projects
            services.AddCustomServiceDependencies(configuration);
            services.AddCacheingConfig(configuration);
            services.AddStripeConfig(configuration);
            services.AddCorsConfig(configuration);
            services.AddRateLimitingConfig();
            services.AddSwaggerConfig();




            return services;
        }

        public static IServiceCollection AddCustomServiceDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Add custom service dependencies (core, infrastructure, etc.)
            services.AddServiceDependencies(configuration);
            services.AddCoreDependencies();
            services.AddInfrastructureDependencies();
            services.AddDataDependencies();

            return services;
        }

        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Your API Title",
                    Version = "v1"
                });

                // Configure Bearer token for Swagger
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by your token"
                });

                // Add security requirement for Bearer token
                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });


            return services;
        }

        public static IServiceCollection AddStripeConfig(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure Stripe settings
            services.Configure<StripeSettings>(configuration.GetSection("Stripe"));

            // Register StripeClient
            services.AddSingleton(provider =>
            {
                var stripeSettings = provider.GetRequiredService<IOptions<StripeSettings>>().Value;
                return new StripeClient(stripeSettings.SecretKey);
            });

            return services;
        }

        public static IServiceCollection AddCorsConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyMethod()
                           .AllowAnyHeader();

                    if (allowedOrigins != null && allowedOrigins.Length > 0)
                    {
                        builder.WithOrigins(allowedOrigins);
                    }
                    else
                    {
                        builder.AllowAnyOrigin(); // Or configure as needed for no origins specified
                    }
                });
            });

            return services;
        }

        private static IServiceCollection AddRateLimitingConfig(this IServiceCollection services)
        {
            services.AddRateLimiter(rateLimiterOptions =>
            {
                rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                // Limits requests from the same IP address to 2 requests every 20 seconds;
                rateLimiterOptions.AddPolicy(RateLimiters.IpLimiter, httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 2,
                            Window = TimeSpan.FromSeconds(20)
                        }
                )
                );

                // Limits requests from the same user ID to 2 requests every 20 seconds
                rateLimiterOptions.AddPolicy(RateLimiters.UserLimiter, httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.User.GetUserId(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 2,
                            Window = TimeSpan.FromSeconds(20)
                        }
                )
                );

                // Limits the number of concurrent requests to 1000 and allows up to 100 requests in the queue
                rateLimiterOptions.AddConcurrencyLimiter(RateLimiters.Concurrency, options =>
                {
                    options.PermitLimit = 1000;
                    options.QueueLimit = 100;
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });
            });

            return services;
        }

        private static IServiceCollection AddCacheingConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConfiguration = configuration["RedisCacheServerUrl"]; // You can choose which key to use
            var instanceName = configuration["Redis:InstanceName"] ?? "ELearning_"; // Default instance name

            // Add Redis Cache
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConfiguration; // Redis server configuration from appsettings.json
                options.InstanceName = instanceName; // Prefix for keys stored in Redis (optional)
            });

            return services;
        }
    }
}

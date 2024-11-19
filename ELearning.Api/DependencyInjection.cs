using ELearning.Core.Base.ApiResponse;
using ELearning.Core.MediatrHandlers.Student.Queries.GetAllStudents;
using ELearning.Data;
using System.Reflection;

namespace ELearning.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            // Configuration of MediatR (registering from the current assembly)
          //  services.AddMediatR(Assembly.GetExecutingAssembly());




            // Configure database connection
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            //Dependencies of projects
            services.AddCustomServiceDependencies(configuration);
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
    }
}

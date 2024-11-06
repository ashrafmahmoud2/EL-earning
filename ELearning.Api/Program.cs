using ELearning.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ELearning.Service;
using MediatR;
using System.Reflection;
using ELearning.Core.MediatrHandlers.Student.Queries.GetStudentById;
using ELearning.Infrastructure.Base;
using AutoMapper;
using ELearning.Core;
using Hangfire;
using HangfireBasicAuthenticationFilter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Your API Title",
        Version = "v1"
    });

    // Add security definition for Bearer token
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your token"
    });

    // Add security requirement
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


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
          throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));



builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddServiceDependencies(builder.Configuration);
builder.Services.AddCoreDependencies();
builder.Services.AddInfrastructureDependencies();


builder.Services.AddMediatR(typeof(GetStudentByIdQuery).Assembly);

var app = builder.Build();



//app.UseHangfireDashboard("/jobs", new DashboardOptions
//{
//    Authorization =
//    [
//        new HangfireCustomBasicAuthenticationFilter
//        {
//            User = app.Configuration.GetValue<string>("HangfireSettings:Username"),
//            Pass = app.Configuration.GetValue<string>("HangfireSettings:Password")
//        }
//    ],
//    DashboardTitle = "Survey Basket Dashboard",
//});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
          throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));



builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddServiceDependencies();
builder.Services.AddCoreDependencies();
builder.Services.AddInfrastructureDependencies();


builder.Services.AddMediatR(typeof(GetStudentByIdQuery).Assembly);

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

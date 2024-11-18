using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ELearning.Core;
public static class CoreDependencies
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
    {

        // Configuration of MediatR
      //  services.AddMediatR(Assembly.GetExecutingAssembly());

        // Configuration of AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Register FluentValidation
        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        

        services.AddTransient<ApiResponseHandler>();
        return services;
    }

}

using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Data;
public static class DataDependencies
{
    public static IServiceCollection AddDataDependencies(this IServiceCollection services)
    {
        services
          .AddFluentValidationAutoValidation()
          .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }

}
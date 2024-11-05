using ELearning.Infrastructure.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.Infrastructure;

public static class InfrastructureDependencies
{
    public static void AddInfrastructureDependencies(this IServiceCollection services)
    {
        // Register UnitOfWork and GenericRepository
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    }
}


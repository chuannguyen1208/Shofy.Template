using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shofy.UseCases
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DependencyInjection));

            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });

            return services;
        }
    }
}

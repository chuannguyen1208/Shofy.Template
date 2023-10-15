using Microsoft.Extensions.DependencyInjection;
using Shofy.UseCases.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsAdapter
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDocumentsAdapter(this IServiceCollection services)
        {
            services.AddSingleton<IDocumentAdapter, DocumentAdapter>();
            return services;
        }
    }
}

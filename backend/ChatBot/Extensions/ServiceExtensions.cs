using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace ChatBot.Extensions;

// Reference: https://dev.to/tomfletcher9/net-6-register-services-using-reflection-3156
public static class ServiceExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        // Define types that need matching
        Type singletonService = typeof(SingletonServiceAttribute);
        Type transientService = typeof(TransientServiceAttribute);
        Type scopedService = typeof(ScopedServiceAttribute);

        // Later refactor repositories to not be a service
        Type repository = typeof(RepositoryAttribute);

        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => !p.IsInterface &&
                        (p.IsDefined(scopedService, true) ||
                         p.IsDefined(transientService, true) ||
                         p.IsDefined(singletonService, true) ||
                         p.IsDefined(repository, true)))
            .Select(s => new
            {
                Service = s.GetInterface($"I{s.Name}"),
                Implementation = s
            }).Where(x => x.Service != null);


        foreach (var type in types)
        {
            if (type.Implementation.IsDefined(repository, false))
            {
                services.AddSingleton(type.Service!, type.Implementation);
            }

            if (type.Implementation.IsDefined(scopedService, false))
            {
                services.AddScoped(type.Service!, type.Implementation);
            }

            if (type.Implementation.IsDefined(transientService, false))
            {
                services.AddTransient(type.Service!, type.Implementation);
            }

            if (type.Implementation.IsDefined(singletonService, false))
            {
                services.AddSingleton(type.Service!, type.Implementation);
            }
        }
    }
}
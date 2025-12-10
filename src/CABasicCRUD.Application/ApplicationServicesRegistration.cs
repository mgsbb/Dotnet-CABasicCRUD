using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CABasicCRUD.Application;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        var applicationAssembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(applicationAssembly)
        );

        return services;
    }
}

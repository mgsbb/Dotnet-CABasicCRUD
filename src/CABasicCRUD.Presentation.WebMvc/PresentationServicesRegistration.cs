using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Presentation.WebMvc.Common.Security;

namespace CABasicCRUD.Presentation.WebMvc;

public static class PresentationServicesRegistration
{
    public static IServiceCollection RegisterPresentationServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddControllersWithViews();

        return services;
    }
}

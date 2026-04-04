using Microsoft.Extensions.DependencyInjection;
using SidraHub.Application.Services.ServiceCategories;

namespace SidraHub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IServiceCategoryService, ServiceCategoryService>();

        return services;
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SidraHub.Infrastructure.Identity;

namespace SidraHub.Infrastructure.Extensions;

public static class WebApplicationExtensions
{
    public static async Task<WebApplication> SeedIdentityAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IdentityDataSeeder>();
        await seeder.SeedAsync();
        return app;
    }
}

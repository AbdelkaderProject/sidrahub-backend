using Microsoft.Extensions.DependencyInjection;
using SidraHub.Application.Services.ArticleComments;
using SidraHub.Application.Services.Articles;
using SidraHub.Application.Services.Branches;
using SidraHub.Application.Services.CompanyProfiles;
using SidraHub.Application.Services.Partners;
using SidraHub.Application.Services.ServicePackages;
using SidraHub.Application.Services.ServiceSlots;
using SidraHub.Application.Services.ServiceCategories;
using SidraHub.Application.Services.Services;
using SidraHub.Application.Services.Sidebars;
using SidraHub.Application.Services.TeamMembers;

namespace SidraHub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IServiceCategoryService, ServiceCategoryService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<IServicePackageService, ServicePackageService>();
        services.AddScoped<IServiceSlotService, ServiceSlotService>();
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<IArticleCommentService, ArticleCommentService>();
        services.AddScoped<ISidebarService, SidebarService>();
        services.AddScoped<ICompanyProfileService, CompanyProfileService>();
        services.AddScoped<ITeamMemberService, TeamMemberService>();
        services.AddScoped<IPartnerService, PartnerService>();
        services.AddScoped<IBranchService, BranchService>();

        return services;
    }
}

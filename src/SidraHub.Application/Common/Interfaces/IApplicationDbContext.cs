using Microsoft.EntityFrameworkCore;
using SidraHub.Domain.Entities;

namespace SidraHub.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ServiceCategory> ServiceCategories { get; }
    DbSet<Service> Services { get; }
    DbSet<ServicePackage> ServicePackages { get; }
    DbSet<ServiceSlot> ServiceSlots { get; }
    DbSet<AboutPageContent> AboutPageContents { get; }
    DbSet<Article> Articles { get; }
    DbSet<ArticleComment> ArticleComments { get; }
    DbSet<Sidebar> Sidebars { get; }
    DbSet<CompanyProfile> CompanyProfiles { get; }
    DbSet<Customer> Customers { get; }
    DbSet<CustomerReview> CustomerReviews { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<Review> Reviews { get; }
    DbSet<TeamMember> TeamMembers { get; }
    DbSet<Partner> Partners { get; }
    DbSet<Branch> Branches { get; }
    DbSet<ServiceRequest> ServiceRequests { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}


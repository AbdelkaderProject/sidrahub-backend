using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Common;
using SidraHub.Domain.Entities;
using SidraHub.Domain.Enums;
using SidraHub.Infrastructure.Identity;

namespace SidraHub.Infrastructure.Persistence;

public sealed class SidraHubDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SidraHubDbContext(DbContextOptions<SidraHubDbContext> options)
        : this(options, new HttpContextAccessor())
    {
    }

    public SidraHubDbContext(
        DbContextOptions<SidraHubDbContext> options,
        IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<ServiceCategory> ServiceCategories => Set<ServiceCategory>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<ServicePackage> ServicePackages => Set<ServicePackage>();
    public DbSet<ServiceSlot> ServiceSlots => Set<ServiceSlot>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<ArticleComment> ArticleComments => Set<ArticleComment>();
    public DbSet<Sidebar> Sidebars => Set<Sidebar>();
    public DbSet<CompanyProfile> CompanyProfiles => Set<CompanyProfile>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<CustomerReview> CustomerReviews => Set<CustomerReview>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<Partner> Partners => Set<Partner>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();

    public override int SaveChanges()
    {
        ApplyAuditRules();
        ApplySoftDeleteRules();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditRules();
        ApplySoftDeleteRules();
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("Users");
            entity.Property(x => x.FullNameAr).HasMaxLength(255);
            entity.Property(x => x.Note).HasColumnType("nvarchar(max)");
        });

        builder.Entity<ServiceCategory>(entity =>
        {
            entity.ToTable("ServiceCategory");
            ConfigureAuditableEntity(entity);
            entity.Property(x => x.NameEn).HasMaxLength(255).IsRequired();
            entity.Property(x => x.NameAr).HasMaxLength(255).IsRequired();
        });

        builder.Entity<Service>(entity =>
        {
            entity.ToTable("Service");
            ConfigureAuditableEntity(entity);
            entity.Property(x => x.NameEn).HasMaxLength(255).IsRequired();
            entity.Property(x => x.NameAr).HasMaxLength(255).IsRequired();
            entity.Property(x => x.ShortDescriptionEn).HasMaxLength(255).IsRequired();
            entity.Property(x => x.ShortDescriptionAr).HasMaxLength(255).IsRequired();
            entity.Property(x => x.DescriptionEn).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.DescriptionAr).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.Icon).HasMaxLength(500);
            entity.HasOne(x => x.ServiceCategory)
                .WithMany(x => x.Services)
                .HasForeignKey(x => x.ServiceCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<ServicePackage>(entity =>
        {
            entity.ToTable("ServicePackage");
            ConfigureAuditableEntity(entity);
            entity.Property(x => x.NameEn).HasMaxLength(255).IsRequired();
            entity.Property(x => x.NameAr).HasMaxLength(255).IsRequired();
            entity.Property(x => x.Icon).HasMaxLength(500);
            entity.Property(x => x.CostAmount).HasPrecision(18, 2);
            entity.HasOne(x => x.Service)
                .WithMany(x => x.ServicePackages)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<ServiceSlot>(entity =>
        {
            entity.ToTable("ServiceSlot");
            ConfigureAuditableEntity(entity);
            entity.Property(x => x.Day).HasColumnType("date");
            entity.Property(x => x.TimeFrom).HasColumnType("time");
            entity.Property(x => x.TimeTo).HasColumnType("time");
            entity.HasOne(x => x.Service)
                .WithMany(x => x.ServiceSlots)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        builder.Entity<Article>(entity =>
        {
            entity.ToTable("Articles");
            ConfigureAuditableEntity(entity);
            entity.Property(x => x.ServiceCategoryId).HasColumnType("int");
            entity.Property(x => x.TitleEn).HasMaxLength(255).IsRequired();
            entity.Property(x => x.TitleAr).HasMaxLength(255).IsRequired();
            entity.Property(x => x.ShortDescriptionEn).HasMaxLength(255).IsRequired();
            entity.Property(x => x.ShortDescriptionAr).HasMaxLength(255).IsRequired();
            entity.Property(x => x.DescriptionEn).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.DescriptionAr).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.Image).HasMaxLength(500);
            entity.HasOne(x => x.ServiceCategory)
                .WithMany()
                .HasForeignKey(x => x.ServiceCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<ArticleComment>(entity =>
        {
            entity.ToTable("ArticlesComment");
            ConfigureAuditableEntity(entity);
            entity.Property(x => x.CommentContent).HasColumnType("nvarchar(max)").IsRequired();
            entity.HasOne(x => x.Article)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Sidebar>(entity =>
        {
            entity.ToTable("Sidebar");
            ConfigureAuditableEntity(entity);
            entity.Property(x => x.TitleEn).HasMaxLength(255).IsRequired();
            entity.Property(x => x.TitleAr).HasMaxLength(255).IsRequired();
            entity.Property(x => x.DescriptionEn).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.DescriptionAr).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.Image).HasMaxLength(500);
            entity.HasOne(x => x.Service)
                .WithMany(x => x.Sidebars)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<CompanyProfile>(entity =>
        {
            entity.ToTable("CompanyProfile");
            ConfigureAuditableEntity(entity);
            entity.Property(x => x.NameEn).HasMaxLength(255).IsRequired();
            entity.Property(x => x.NameAr).HasMaxLength(255).IsRequired();
            entity.Property(x => x.Logo).HasMaxLength(500);
            entity.Property(x => x.DescriptionEn).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.DescriptionAr).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.InsgramLinkStr).HasMaxLength(500);
            entity.Property(x => x.FacebookLinkStr).HasMaxLength(500);
            entity.Property(x => x.TwitterLinkStr).HasMaxLength(500);
            entity.Property(x => x.LinkdInLinkStr).HasMaxLength(500);
            entity.Property(x => x.WhatsApp).HasMaxLength(255);
        });

        builder.Entity<CustomerReview>(entity =>
        {
            entity.ToTable("CustomerReview");
            ConfigureAuditableEntity(entity);
            entity.Property(x => x.NameEn).HasMaxLength(255).IsRequired();
            entity.Property(x => x.NameAr).HasMaxLength(255).IsRequired();
            entity.Property(x => x.OpinionEn).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.OpinionAr).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.URLStr).HasMaxLength(500);
        });

        builder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");
            ConfigureAuditableEntity(entity);
            entity.Property(x => x.NameEn).HasMaxLength(255).IsRequired();
            entity.Property(x => x.NameAr).HasMaxLength(255).IsRequired();
            entity.Property(x => x.Logo).HasMaxLength(500);
        });

        builder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notifications");
            ConfigureAuditableEntity(entity);
            entity.Property(x => x.UserId).HasMaxLength(450).IsRequired();
            entity.Property(x => x.Title).HasMaxLength(255).IsRequired();
            entity.Property(x => x.Message).HasColumnType("nvarchar(max)").IsRequired();
            entity.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Review>(entity =>
        {
            entity.ToTable("Reviews");
            ConfigureAuditableEntity(entity);
            entity.Property(x => x.UserId).HasMaxLength(450).IsRequired();
            entity.Property(x => x.Comment).HasColumnType("nvarchar(max)").IsRequired();
            entity.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<TeamMember>(entity =>
        {
            entity.ToTable("TeamMember");
            ConfigureAuditableEntity(entity);
            entity.Property(x => x.NameEn).HasMaxLength(255).IsRequired();
            entity.Property(x => x.NameAr).HasMaxLength(255).IsRequired();
            entity.Property(x => x.DescriptionEn).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.DescriptionAr).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.Image).HasMaxLength(500);
            entity.Property(x => x.InsgramLinkStr).HasMaxLength(500);
            entity.Property(x => x.FacebookLinkStr).HasMaxLength(500);
            entity.Property(x => x.TwitterLinkStr).HasMaxLength(500);
            entity.Property(x => x.LinkdInLinkStr).HasMaxLength(500);
            entity.Property(x => x.WhatsApp).HasMaxLength(255);
        });

        builder.Entity<Partner>(entity =>
        {
            entity.ToTable("Partners");
            ConfigureAuditableEntity(entity);
            entity.Property(x => x.NameEn).HasMaxLength(255).IsRequired();
            entity.Property(x => x.NameAr).HasMaxLength(255).IsRequired();
            entity.Property(x => x.Logo).HasMaxLength(500);
        });

        builder.Entity<Branch>(entity =>
        {
            entity.ToTable("Branche");
            ConfigureAuditableEntity(entity);
            entity.Property(x => x.NameEn).HasMaxLength(255).IsRequired();
            entity.Property(x => x.NameAr).HasMaxLength(255).IsRequired();
            entity.Property(x => x.AddressEn).HasMaxLength(255).IsRequired();
            entity.Property(x => x.AddressAr).HasMaxLength(255).IsRequired();
            entity.Property(x => x.PhoneNumber).HasMaxLength(255).IsRequired();
        });

        builder.Entity<ServiceRequest>(entity =>
        {
            entity.ToTable("ServiceRequest");
            ConfigureAuditableEntity(entity);
            entity.Property(x => x.Code).HasMaxLength(255).IsRequired();
            entity.Property(x => x.UserId).HasMaxLength(450).IsRequired(false);
            entity.Property(x => x.CustomerName).HasMaxLength(255).IsRequired();
            entity.Property(x => x.CustomerEmail).HasMaxLength(255).IsRequired();
            entity.Property(x => x.CustomerPhone).HasMaxLength(255).IsRequired();
            entity.Property(x => x.Description).HasColumnName("Descrption").HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.RequestDate).HasColumnType("date");
            entity.Property(x => x.RequestTime).HasColumnType("time");
            entity.Property(x => x.RequestStatus)
                .HasConversion<int>()
                .HasColumnType("int");
            entity.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Service)
                .WithMany(x => x.ServiceRequests)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.ServiceSlot)
                .WithMany()
                .HasForeignKey(x => x.ServiceSlotId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        ApplySoftDeleteQueryFilters(builder);
    }

    private static void ConfigureAuditableEntity<TEntity>(EntityTypeBuilder<TEntity> entity)
        where TEntity : BaseEntity
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();
        entity.Property(x => x.CreatedBy).HasColumnName("CreateBy").HasMaxLength(450);
        entity.Property(x => x.CreatedDateTime).HasColumnName("CreatedDateTime");
        entity.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy").HasMaxLength(450);
        entity.Property(x => x.UpdatedDateTime).HasColumnName("UpdatedDateTime");
        entity.Property(x => x.IsDeleted).HasColumnName("IsDetete");
        entity.Property(x => x.DeletedBy).HasColumnName("DeletedBy").HasMaxLength(450);
        entity.Property(x => x.DeletedDateTime).HasColumnName("DeletedDatedTime");
    }

    private void ApplyAuditRules()
    {
        ChangeTracker.DetectChanges();

        var currentUserId = GetCurrentUserId();
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = currentUserId;
                entry.Entity.CreatedDateTime = utcNow;
                entry.Entity.UpdatedBy = null;
                entry.Entity.UpdatedDateTime = null;
                entry.Entity.DeletedBy = null;
                entry.Entity.DeletedDateTime = null;
                continue;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property(entity => entity.CreatedBy).IsModified = false;
                entry.Property(entity => entity.CreatedDateTime).IsModified = false;
                entry.Property(entity => entity.DeletedBy).IsModified = false;
                entry.Property(entity => entity.DeletedDateTime).IsModified = false;

                if (!entry.Entity.IsDeleted)
                {
                    entry.Entity.UpdatedBy = currentUserId;
                    entry.Entity.UpdatedDateTime = utcNow;
                }
            }
        }
    }

    private void ApplySoftDeleteRules()
    {
        ChangeTracker.DetectChanges();

        foreach (var entry in ChangeTracker.Entries<BaseEntity>().Where(entry => entry.State == EntityState.Deleted))
        {
            entry.State = EntityState.Modified;
            entry.Entity.IsDeleted = true;
            entry.Entity.DeletedBy = GetCurrentUserId();
            entry.Entity.DeletedDateTime ??= DateTime.UtcNow;
        }
    }

    private string? GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user is null)
        {
            return null;
        }

        return user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue("sub");
    }

    private static void ApplySoftDeleteQueryFilters(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                continue;
            }

            var method = typeof(SidraHubDbContext)
                .GetMethod(nameof(SetSoftDeleteQueryFilter), BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(entityType.ClrType);

            method.Invoke(null, [builder]);
        }
    }

    private static void SetSoftDeleteQueryFilter<TEntity>(ModelBuilder builder)
        where TEntity : BaseEntity
    {
        builder.Entity<TEntity>().HasQueryFilter(entity => !entity.IsDeleted);
    }
}


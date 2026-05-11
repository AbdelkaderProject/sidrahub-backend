using System.Reflection;
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
    public SidraHubDbContext(DbContextOptions<SidraHubDbContext> options)
        : base(options)
    {
    }

    public DbSet<ServiceCategory> ServiceCategories => Set<ServiceCategory>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<ServicePackage> ServicePackages => Set<ServicePackage>();
    public DbSet<ServiceSlot> ServiceSlots => Set<ServiceSlot>();
    public DbSet<AboutPageContent> AboutPageContents => Set<AboutPageContent>();
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
        ApplySoftDeleteRules();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
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

        builder.Entity<AboutPageContent>(entity =>
        {
            entity.ToTable("AboutPageContent");
            ConfigureAuditableEntity(entity);
            entity.Property(x => x.MainTitleAr).HasMaxLength(255).IsRequired();
            entity.Property(x => x.MainTitleEn).HasMaxLength(255).IsRequired();
            entity.Property(x => x.SubTitleAr).HasMaxLength(255).IsRequired();
            entity.Property(x => x.SubTitleEn).HasMaxLength(255).IsRequired();
            entity.Property(x => x.IntroTextAr).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.IntroTextEn).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.WhyChooseTitleAr).HasMaxLength(255).IsRequired();
            entity.Property(x => x.WhyChooseTitleEn).HasMaxLength(255).IsRequired();
            entity.Property(x => x.WhyChooseDescriptionAr).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.WhyChooseDescriptionEn).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.WhatWeOfferTitleAr).HasMaxLength(255).IsRequired();
            entity.Property(x => x.WhatWeOfferTitleEn).HasMaxLength(255).IsRequired();
            entity.Property(x => x.WhatWeOfferDescriptionAr).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.WhatWeOfferDescriptionEn).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.MissionTitleAr).HasMaxLength(255).IsRequired();
            entity.Property(x => x.MissionTitleEn).HasMaxLength(255).IsRequired();
            entity.Property(x => x.MissionDescriptionAr).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.MissionDescriptionEn).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.WhereWeWorkTitleAr).HasMaxLength(255).IsRequired();
            entity.Property(x => x.WhereWeWorkTitleEn).HasMaxLength(255).IsRequired();
            entity.Property(x => x.WhereWeWorkDescriptionAr).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.WhereWeWorkDescriptionEn).HasColumnType("nvarchar(max)").IsRequired();
            entity.Property(x => x.Image).HasMaxLength(500);
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
            entity.Property(x => x.UserId).HasMaxLength(450).IsRequired();
            entity.Property(x => x.UserName).HasMaxLength(255).IsRequired();
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
            entity.Property(x => x.UserId).HasMaxLength(450).IsRequired();
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
        });

        ApplySoftDeleteQueryFilters(builder);
    }

    private static void ConfigureAuditableEntity<TEntity>(EntityTypeBuilder<TEntity> entity)
        where TEntity : BaseEntity
    {
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Id).ValueGeneratedOnAdd();
        entity.Property(x => x.CreatedBy).HasColumnName("CreateBy");
        entity.Property(x => x.CreatedDateTime).HasColumnName("CreatedDateTime");
        entity.Property(x => x.UpdatedBy).HasColumnName("UpdatedBy");
        entity.Property(x => x.UpdatedDateTime).HasColumnName("UpdatedDateTime");
        entity.Property(x => x.IsDeleted).HasColumnName("IsDetete");
        entity.Property(x => x.DeletedBy).HasColumnName("DeletedBy");
        entity.Property(x => x.DeletedDateTime).HasColumnName("DeletedDatedTime");
    }

    private void ApplySoftDeleteRules()
    {
        ChangeTracker.DetectChanges();

        foreach (var entry in ChangeTracker.Entries<BaseEntity>().Where(entry => entry.State == EntityState.Deleted))
        {
            entry.State = EntityState.Modified;
            entry.Entity.IsDeleted = true;
            entry.Entity.DeletedDateTime ??= DateTime.UtcNow;
        }
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


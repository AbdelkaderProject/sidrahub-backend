using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Entities;
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
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderService> OrderServices => Set<OrderService>();
    public DbSet<Provider> Providers => Set<Provider>();
    public DbSet<ServiceProvider> ServiceProviders => Set<ServiceProvider>();
    public DbSet<Consultation> Consultations => Set<Consultation>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ServiceCategory>(entity =>
        {
            entity.ToTable("ServiceCategories");
            entity.Property(x => x.NameAr).HasMaxLength(150).IsRequired();
            entity.Property(x => x.NameEn).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Icon).HasMaxLength(300);
        });

        builder.Entity<Service>(entity =>
        {
            entity.ToTable("Services");
            entity.Property(x => x.NameAr).HasMaxLength(200).IsRequired();
            entity.Property(x => x.NameEn).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(4000).IsRequired();
            entity.Property(x => x.BasePrice).HasPrecision(18, 2);
            entity.HasOne(x => x.Category)
                .WithMany(x => x.Services)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");
            entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(50);
            entity.Property(x => x.TotalAmount).HasPrecision(18, 2);
            entity.Property(x => x.UserId).HasMaxLength(450).IsRequired();
        });

        builder.Entity<OrderService>(entity =>
        {
            entity.ToTable("OrderServices");
            entity.Property(x => x.PriceAtTime).HasPrecision(18, 2);
            entity.HasOne(x => x.Order)
                .WithMany(x => x.OrderServices)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.Service)
                .WithMany(x => x.OrderServices)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(x => new { x.OrderId, x.ServiceId }).IsUnique();
        });

        builder.Entity<Provider>(entity =>
        {
            entity.ToTable("Providers");
            entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Phone).HasMaxLength(30).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(256).IsRequired();
            entity.Property(x => x.Type).HasConversion<string>().HasMaxLength(50);
        });

        builder.Entity<ServiceProvider>(entity =>
        {
            entity.ToTable("ServiceProviders");
            entity.HasOne(x => x.Service)
                .WithMany(x => x.ServiceProviders)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.Provider)
                .WithMany(x => x.ServiceProviders)
                .HasForeignKey(x => x.ProviderId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(x => new { x.ServiceId, x.ProviderId }).IsUnique();
        });

        builder.Entity<Consultation>(entity =>
        {
            entity.ToTable("Consultations");
            entity.Property(x => x.FullName).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Phone).HasMaxLength(30).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(256).IsRequired();
            entity.Property(x => x.Message).HasMaxLength(3000).IsRequired();
            entity.Property(x => x.UserId).HasMaxLength(450);
            entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(50);
            entity.HasOne(x => x.Service)
                .WithMany(x => x.Consultations)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Article>(entity =>
        {
            entity.ToTable("Articles");
            entity.Property(x => x.TitleAr).HasMaxLength(250).IsRequired();
            entity.Property(x => x.TitleEn).HasMaxLength(250).IsRequired();
            entity.Property(x => x.Slug).HasMaxLength(250).IsRequired();
            entity.Property(x => x.ImageUrl).HasMaxLength(500);
            entity.Property(x => x.Content).HasColumnType("nvarchar(max)").IsRequired();
            entity.HasIndex(x => x.Slug).IsUnique();
        });

        builder.Entity<Review>(entity =>
        {
            entity.ToTable("Reviews");
            entity.Property(x => x.UserId).HasMaxLength(450).IsRequired();
            entity.Property(x => x.Comment).HasMaxLength(1500).IsRequired();
            entity.HasCheckConstraint("CK_Reviews_Rating", "[Rating] >= 1 AND [Rating] <= 5");
            entity.HasOne(x => x.Order)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notifications");
            entity.Property(x => x.UserId).HasMaxLength(450).IsRequired();
            entity.Property(x => x.Title).HasMaxLength(250).IsRequired();
            entity.Property(x => x.Message).HasMaxLength(2000).IsRequired();
        });

        SeedCategories(builder);
    }

    private static void SeedCategories(ModelBuilder builder)
    {
        var companySetupId = Guid.Parse("3efef08d-1f7d-4d45-9ccf-9b5dd20f35a1");
        var legalId = Guid.Parse("d936e779-4744-4c07-8eca-a911e60451c6");
        var accountingId = Guid.Parse("f245a143-d4a1-4768-bfd2-d9d28ec2a60d");
        var hrId = Guid.Parse("c29563dc-d0e5-4a29-8440-017b57d5287e");
        var workspaceId = Guid.Parse("e38f4998-3140-4135-940b-ec7d59811f5c");

        builder.Entity<ServiceCategory>().HasData(
            new ServiceCategory { Id = companySetupId, NameAr = "تأسيس الشركات", NameEn = "Company Setup", DisplayOrder = 1 },
            new ServiceCategory { Id = legalId, NameAr = "الاستشارات القانونية", NameEn = "Legal Services", DisplayOrder = 2 },
            new ServiceCategory { Id = accountingId, NameAr = "الخدمات المحاسبية والضريبية", NameEn = "Accounting & Tax", DisplayOrder = 3 },
            new ServiceCategory { Id = hrId, NameAr = "خدمات الموارد البشرية", NameEn = "HR Services", DisplayOrder = 4 },
            new ServiceCategory { Id = workspaceId, NameAr = "مساحات العمل", NameEn = "Workspaces", DisplayOrder = 5 });
    }
}

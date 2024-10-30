using HostelFinder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;

namespace HostelFinder.Infrastructure.Context;

public class HostelFinderDbContext : DbContext
{
    public DbSet<Hostel?> Hostels { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<RoomDetails> RoomDetails { get; set; }
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<RoomAmenities> RoomAmenities { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<ServiceCost> ServiceCosts { get; set; }
    public DbSet<BlackListToken> BlackListTokens { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }
    public DbSet<WishlistPost> WishlistPosts { get; set; }
    public DbSet<Membership> Memberships { get; set; }
    public DbSet<MembershipServices> MembershipServices { get; set; }
    public DbSet<UserMembership> UserMemberships { get; set; }
    public DbSet<Invoice> InVoices { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Address> Addresses { get; set; }


    public DbSet<HostelService> HostelServices { get; set; }

    public HostelFinderDbContext(DbContextOptions<HostelFinderDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configurationRoot = builder.Build();
            optionsBuilder.UseSqlServer(configurationRoot.GetConnectionString("DefaultConnection"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Address entity
        modelBuilder.Entity<Address>()
            .HasKey(a => a.HostelId);
        modelBuilder.Entity<Address>()
            .HasOne(a => a.Hostel)
            .WithOne(h => h.Address)
            .HasForeignKey<Address>(a => a.HostelId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Amenity entity
        modelBuilder.Entity<Amenity>()
            .HasKey(a => a.Id);

        // Configure Hostel entity
        modelBuilder.Entity<Hostel>()
            .HasOne(h => h.Landlord)
            .WithMany(u => u.Hostels)
            .HasForeignKey(h => h.LandlordId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Hostel>()
            .HasMany(h => h.HostelServices)
            .WithOne(s => s.Hostel)
            .HasForeignKey(s => s.HostelId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Hostel>()
            .HasMany(h => h.Posts)
            .WithOne(p => p.Hostel)
            .HasForeignKey(p => p.HostelId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Hostel>()
            .HasMany(h => h.Images)
            .WithOne(i => i.Hostel)
            .HasForeignKey(i => i.HostelId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Hostel>()
            .HasMany(h => h.Rooms)
            .WithOne(r => r.Hostel)
            .HasForeignKey(r => r.HostelId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Image entity
        modelBuilder.Entity<Image>()
            .HasKey(i => i.Id);
        modelBuilder.Entity<Image>()
            .HasOne(i => i.Hostel)
            .WithMany(h => h.Images)
            .HasForeignKey(i => i.HostelId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Image>()
            .HasOne(i => i.Post)
            .WithMany(p => p.Images)
            .HasForeignKey(i => i.PostId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Membership entity
        modelBuilder.Entity<Membership>()
            .HasKey(m => m.Id);

        // Configure MembershipServices entity
        modelBuilder.Entity<MembershipServices>()
            .HasKey(ms => ms.Id);
        modelBuilder.Entity<MembershipServices>()
            .HasMany(ms => ms.Posts)
            .WithOne(p => p.MembershipServices)
            .HasForeignKey(p => p.MembershipServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Post entity
        modelBuilder.Entity<Post>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<Post>()
            .HasOne(p => p.Hostel)
            .WithMany(h => h.Posts)
            .HasForeignKey(p => p.HostelId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Post>()
            .HasOne(p => p.Room)
            .WithMany(r => r.Posts)
            .HasForeignKey(p => p.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Room entity
        modelBuilder.Entity<Room>()
            .HasKey(r => r.Id);
        modelBuilder.Entity<Room>()
            .HasMany(r => r.RoomAmenities)
            .WithOne(ra => ra.Room)
            .HasForeignKey(ra => ra.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Room>()
            .HasMany(r => r.Posts)
            .WithOne(p => p.Room)
            .HasForeignKey(p => p.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Room>()
            .HasOne(r => r.Hostel)
            .WithMany(h => h.Rooms)
            .HasForeignKey(r => r.HostelId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure RoomAmenities entity
        modelBuilder.Entity<RoomAmenities>()
            .HasKey(ra => new { ra.RoomId, ra.AmenityId });
        modelBuilder.Entity<RoomAmenities>()
            .HasOne(ra => ra.Room)
            .WithMany(r => r.RoomAmenities)
            .HasForeignKey(ra => ra.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<RoomAmenities>()
            .HasOne(ra => ra.Amenity)
            .WithMany(a => a.RoomAmenities)
            .HasForeignKey(ra => ra.AmenityId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure RoomDetails entity
        modelBuilder.Entity<RoomDetails>()
            .HasKey(rd => rd.PostId);
        modelBuilder.Entity<RoomDetails>()
            .HasOne(rd => rd.Room)
            .WithOne(r => r.RoomDetails)
            .HasForeignKey<RoomDetails>(rd => rd.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Service entity
        modelBuilder.Entity<Service>()
            .HasKey(s => s.Id);
        modelBuilder.Entity<Service>()
            .HasMany(s => s.HostelServices)
            .WithOne(h => h.Services)
            .HasForeignKey(s => s.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure ServiceCost entity
        modelBuilder.Entity<ServiceCost>()
            .HasKey(sc => sc.Id);
        modelBuilder.Entity<ServiceCost>()
            .HasOne(sc => sc.Room)
            .WithMany(r => r.ServiceCost)
            .HasForeignKey(sc => sc.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<ServiceCost>()
            .HasOne(sc => sc.Invoice)
            .WithMany(i => i.ServiceCost)
            .HasForeignKey(sc => sc.InVoiceId)
            .OnDelete(DeleteBehavior.Restrict); // or Cascade if deletion should propagate


        // Configure User entity
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);
        modelBuilder.Entity<User>()
            .HasMany(u => u.Hostels)
            .WithOne(h => h.Landlord)
            .HasForeignKey(h => h.LandlordId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<User>()
            .HasMany(u => u.UserMemberships)
            .WithOne(um => um.User)
            .HasForeignKey(um => um.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<User>()
            .HasOne(u => u.Wishlists)
            .WithOne(w => w.User)
            .HasForeignKey<Wishlist>(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure UserMembership entity
        modelBuilder.Entity<UserMembership>()
            .HasKey(um => new { um.UserId, um.MembershipId });
        modelBuilder.Entity<UserMembership>()
            .HasOne(um => um.User)
            .WithMany(u => u.UserMemberships)
            .HasForeignKey(um => um.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserMembership>()
            .HasOne(um => um.Membership)
            .WithMany(m => m.UserMemberships)
            .HasForeignKey(um => um.MembershipId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Wishlist entity
        modelBuilder.Entity<Wishlist>()
            .HasKey(w => w.Id);
        modelBuilder.Entity<Wishlist>()
            .HasMany(w => w.WishlistPosts)
            .WithOne(wp => wp.Wishlist)
            .HasForeignKey(wp => wp.WishlistId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure WishlistPost entity
        modelBuilder.Entity<WishlistPost>()
            .HasKey(wp => new { wp.WishlistId, wp.PostId });
        modelBuilder.Entity<WishlistPost>()
            .HasOne(wp => wp.Wishlist)
            .WithMany(w => w.WishlistPosts)
            .HasForeignKey(wp => wp.WishlistId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<WishlistPost>()
            .HasOne(wp => wp.Post)
            .WithMany(p => p.WishlistPosts)
            .HasForeignKey(wp => wp.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        // Specify precision for decimal properties
        modelBuilder.Entity<Invoice>()
            .Property(i => i.TotalAmount)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Room>()
            .Property(r => r.MonthlyRentCost)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<ServiceCost>()
            .Property(sc => sc.Cost)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<ServiceCost>()
            .Property(sc => sc.unitCost)
            .HasColumnType("decimal(18,2)");
    }
}
using HostelFinder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HostelFinder.Infrastructure.Context;

public class HostelFinderDbContext : DbContext
{
    public DbSet<BookingRequest> BookingRequests { get; set; }
    public DbSet<Hostel> Hostels { get; set; }
    public DbSet<Review> Reviews { get; set; }
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
    public DbSet<WishlistRoom> WishlistRooms { get; set; }
    public DbSet<Membership> Memberships { get; set; }
    public DbSet<Membership_Services> MembershipServices { get; set; }
    public DbSet<UserMembership> UserMemberships { get; set; }

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

        // Hostel
        modelBuilder.Entity<Hostel>()
            .HasOne(h => h.Landlord)
            .WithMany(u => u.Hostels)
            .HasForeignKey(h => h.LandlordId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Hostel>()
            .HasMany(h => h.Services)
            .WithOne(s => s.Hostel)
            .HasForeignKey(s => s.HostelId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Hostel>()
            .HasMany(h => h.Posts)
            .WithOne(r => r.Hostel)
            .HasForeignKey(r => r.HostelId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Hostel>()
            .HasMany(h => h.Reviews)
            .WithOne(r => r.Hostel)
            .HasForeignKey(r => r.HostelId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Hostel>()
            .HasOne(h => h.Address)
            .WithOne(h => h.Hostel)
            .HasForeignKey<Address>(h => h.HostelId)
            .OnDelete(DeleteBehavior.Cascade);

        // Post
        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasOne(r => r.Hostel)
                .WithMany(h => h.Posts)
                .HasForeignKey(r => r.HostelId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(r => r.BookingRequests)
                .WithOne(br => br.Post)
                .HasForeignKey(br => br.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.RoomDetails)
                .WithOne(rd => rd.Post)
                .HasForeignKey<RoomDetails>(rd => rd.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(r => r.Images)
                .WithOne(i => i.Post)
                .HasForeignKey(i => i.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(r => r.ServiceCosts)
                .WithOne(sc => sc.Post)
                .HasForeignKey(sc => sc.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.Size)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.MonthlyRentCost)
                .HasColumnType("decimal(18,2)");
        });

        //WishlistRoom
        modelBuilder.Entity<WishlistRoom>()
            .HasKey(wr => new { wr.PostId, wr.WishlistId });

        modelBuilder.Entity<WishlistRoom>()
            .HasOne(wr => wr.Post)
            .WithMany(r => r.WishlistRooms)
            .HasForeignKey(wr => wr.PostId);

        modelBuilder.Entity<WishlistRoom>()
            .HasOne(wr => wr.Wishlist)
            .WithMany(w => w.WishlistRooms)
            .HasForeignKey(wr => wr.WishlistId);

        // Review
        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Hostel)
            .WithMany(h => h.Reviews)
            .HasForeignKey(r => r.HostelId)
            .OnDelete(DeleteBehavior.Restrict);

        // BookingRequest
        modelBuilder.Entity<BookingRequest>()
            .HasKey(br => br.RequestId);

        modelBuilder.Entity<BookingRequest>()
            .HasOne(br => br.Post)
            .WithMany(r => r.BookingRequests)
            .HasForeignKey(br => br.PostId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BookingRequest>()
            .HasOne(br => br.User)
            .WithMany(u => u.BookingRequests)
            .HasForeignKey(br => br.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // RoomAmenities configuration
        modelBuilder.Entity<RoomAmenities>()
            .HasKey(ra => new { ra.PostId, ra.AmenityId });

        modelBuilder.Entity<RoomAmenities>()
            .HasOne(ra => ra.Post)
            .WithMany(r => r.RoomAmenities)
            .HasForeignKey(ra => ra.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RoomAmenities>()
            .HasOne(ra => ra.Amenity)
            .WithMany(a => a.RoomAmenities)
            .HasForeignKey(ra => ra.AmenityId)
            .OnDelete(DeleteBehavior.Restrict);

        // Service
        modelBuilder.Entity<Service>()
            .HasOne(s => s.Hostel)
            .WithMany(h => h.Services)
            .HasForeignKey(s => s.HostelId)
            .OnDelete(DeleteBehavior.Restrict);

        // User
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<User>()
            .HasMany(u => u.BookingRequests)
            .WithOne(br => br.User)
            .HasForeignKey(br => br.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Reviews)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Hostels)
            .WithOne(h => h.Landlord)
            .HasForeignKey(h => h.LandlordId)
            .OnDelete(DeleteBehavior.Restrict);

        // Image
        modelBuilder.Entity<Image>()
            .HasOne(i => i.Hostel)
            .WithMany(h => h.Images)
            .HasForeignKey(i => i.HostelId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Image>()
            .HasOne(i => i.Post)
            .WithMany(r => r.Images)
            .HasForeignKey(i => i.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        // Many-to-Many relationship between User and Membership
        modelBuilder.Entity<UserMembership>()
            .HasKey(um => new { um.UserId, um.MembershipId });

        modelBuilder.Entity<UserMembership>()
              .HasOne(um => um.User)
              .WithMany(u => u.UserMemberships)
              .HasForeignKey(um => um.UserId)
              .OnDelete(DeleteBehavior.Restrict);
        

        modelBuilder.Entity<UserMembership>()
            .HasOne(um => um.Membership)
            .WithMany(m => m.UserMemberships)
            .HasForeignKey(um => um.MembershipId)
            .OnDelete(DeleteBehavior.Restrict);

        //Membership
        modelBuilder.Entity<Membership>()
               .HasMany(m => m.Membership_Services)
               .WithOne(ms => ms.Membership)
               .HasForeignKey(ms => ms.MembershipId)
               .OnDelete(DeleteBehavior.Cascade);


        //Membership_Services
        modelBuilder.Entity<Membership_Services>()
               .HasMany(ms => ms.Posts)
               .WithOne(p => p.Membership_Services)
               .HasForeignKey(p => p.MembershipServiceId)
               .OnDelete(DeleteBehavior.Restrict);

        // RoomDetails
        modelBuilder.Entity<RoomDetails>(entity =>
        {
            entity.Property(e => e.Size)
                .HasColumnType("decimal(18,2)");
        });

        // ServiceCost
        modelBuilder.Entity<ServiceCost>(entity =>
        {
            entity.Property(e => e.Cost)
                .HasColumnType("decimal(18,2)");
        });


    }
}
using HostelFinder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HostelFinder.Infrastructure.Context;

public class HostelFinderDbContext : DbContext
{
    public DbSet<BookingRequest> BookingRequests { get; set; }
    public DbSet<Hostel> Hostels { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomDetails> RoomDetails { get; set; }
    public DbSet<RoomAmenities> RoomAmenities { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<ServiceCost> ServiceCosts { get; set; }
    public DbSet<BlackListToken> BlackListTokens { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }
    public DbSet<WishlistRoom> WishlistRooms { get; set; }


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
            .HasMany(h => h.Rooms)
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

        // Room
        modelBuilder.Entity<Room>()
            .HasOne(r => r.Hostel)
            .WithMany(h => h.Rooms)
            .HasForeignKey(r => r.HostelId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Room>()
            .HasMany(r => r.BookingRequests)
            .WithOne(br => br.Room)
            .HasForeignKey(br => br.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Room>()
            .HasOne(r => r.RoomDetails)
            .WithOne(rf => rf.Room)
            .HasForeignKey<RoomDetails>(rf => rf.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Room>()
            .HasOne(r => r.RoomAmenities)
            .WithOne(ra => ra.Room)
            .HasForeignKey<RoomAmenities>(ra => ra.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Room>()
            .HasMany(r => r.Images)
            .WithOne(i => i.Room)
            .HasForeignKey(i => i.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Room>(entity =>
        {
            entity.Property(e => e.Size)
                .HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.Property(e => e.MonthlyRentCost)
                .HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Room>()
            .HasMany(r => r.ServiceCosts)
            .WithOne(sc => sc.Room)
            .HasForeignKey(sc => sc.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        //WishlistRoom
        modelBuilder.Entity<WishlistRoom>()
            .HasKey(wr => new { wr.RoomId, wr.WishlistId });

        modelBuilder.Entity<WishlistRoom>()
            .HasOne(wr => wr.Room)
            .WithMany(r => r.WishlistRooms)
            .HasForeignKey(wr => wr.RoomId);

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
            .HasOne(br => br.Room)
            .WithMany(r => r.BookingRequests)
            .HasForeignKey(br => br.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BookingRequest>()
            .HasOne(br => br.User)
            .WithMany(u => u.BookingRequests)
            .HasForeignKey(br => br.UserId)
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
            .HasOne(i => i.Room)
            .WithMany(r => r.Images)
            .HasForeignKey(i => i.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

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
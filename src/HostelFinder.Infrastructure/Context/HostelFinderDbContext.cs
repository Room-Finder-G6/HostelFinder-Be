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
    public DbSet<RoomFeature> RoomFeatures { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Image> Images { get; set; }
    
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

    // Room
    modelBuilder.Entity<Room>()
        .HasOne(r => r.Hostel)
        .WithMany(h => h.Rooms)
        .HasForeignKey(r => r.HostelId)
        .OnDelete(DeleteBehavior.Restrict); 

    modelBuilder.Entity<Room>()
        .HasOne(r => r.RoomType)
        .WithMany(rt => rt.Rooms)
        .HasForeignKey(r => r.RoomTypeId)
        .OnDelete(DeleteBehavior.Restrict); 

    modelBuilder.Entity<Room>()
        .HasMany(r => r.BookingRequests)
        .WithOne(br => br.Room)
        .HasForeignKey(br => br.RoomId)
        .OnDelete(DeleteBehavior.Restrict); 

    modelBuilder.Entity<Room>()
        .HasMany(r => r.RoomFeatures)
        .WithOne(rf => rf.Room)
        .HasForeignKey(rf => rf.RoomId)
        .OnDelete(DeleteBehavior.Restrict); 

    modelBuilder.Entity<Room>()
        .HasMany(r => r.Images)
        .WithOne(i => i.Room)
        .HasForeignKey(i => i.RoomId)
        .OnDelete(DeleteBehavior.Restrict); 

    // RoomType

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

    // RoomFeature
    modelBuilder.Entity<RoomFeature>()
        .HasKey(rf => rf.Id);

    modelBuilder.Entity<RoomFeature>()
        .HasOne(rf => rf.Room)
        .WithMany(r => r.RoomFeatures)
        .HasForeignKey(rf => rf.RoomId)
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
        .OnDelete(DeleteBehavior.Restrict);
}
}
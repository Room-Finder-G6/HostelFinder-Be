
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Enums;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Seeders
{
    public class HostelSeeder(HostelFinderDbContext dbContext) : IHostelSeeder
    {
        public async Task Seed()
        {
            if (dbContext.Database.GetPendingMigrations().Any())
            {
                await dbContext.Database.MigrateAsync();
            }
            if (await dbContext.Database.CanConnectAsync())
            {
                if (await dbContext.Users.FirstOrDefaultAsync(x => x.Role == UserRole.Admin) == null)
                {
                    var admin = new User
                    {
                        Username = "admin",
                        Password = "admin@123",
                        Role = UserRole.Admin,
                        Email = "matchfinder.center@gmail.com",
                        Phone = "06868686868",
                        IsActive = true,
                        IsEmailConfirmed = true,
                    };
                    await dbContext.Users.AddAsync(admin);
                    await dbContext.SaveChangesAsync();
                }
                if (!dbContext.Services.Any())
                {
                    var service = GetServices();
                    dbContext.Services.AddRange(service);
                    await dbContext.SaveChangesAsync();
                }
                if (!dbContext.Amenities.Any())
                {
                    var amenities = GetAmenities();
                    dbContext.Amenities.AddRange(amenities);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
        private IEnumerable<Service> GetServices()
        {
            List<Service> services = [
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "Bảo vệ",
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "Camera",
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "Bãi để xe",
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "Thang máy",
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "Internet",
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "Wifi",
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "Giặt là",
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "WC",
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false
                },
                ];
            return services;
        }
        private IEnumerable<Amenity> GetAmenities()
        {
            List<Amenity> amenities = [
                new() {
                    Id = Guid.NewGuid(),
                    AmenityName = "Tivi",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    CreatedBy = "Hệ thống",
                },
                new() {
                    Id = Guid.NewGuid(),
                    AmenityName = "Ban công",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    CreatedBy = "Hệ thống",
                },
                new() {
                    Id = Guid.NewGuid(),
                    AmenityName = "Gác xép",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    CreatedBy = "Hệ thống",
                },
                new() {
                    Id = Guid.NewGuid(),
                    AmenityName = "Giường",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    CreatedBy = "Hệ thống",
                },
                new() {
                    Id = Guid.NewGuid(),
                    AmenityName = "Bình nóng lạnh",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    CreatedBy = "Hệ thống",
                },
                new() {
                    Id = Guid.NewGuid(),
                    AmenityName = "Tủ quần áo",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    CreatedBy = "Hệ thống",
                },
                new() {
                    Id = Guid.NewGuid(),
                    AmenityName = "Vệ sinh chung",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    CreatedBy = "Hệ thống",
                },
                new() {
                    Id = Guid.NewGuid(),
                    AmenityName = "Vệ sinh khép kín",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    CreatedBy = "Hệ thống",
                },
                new() {
                    Id = Guid.NewGuid(),
                    AmenityName = "Bàn học",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    CreatedBy = "Hệ thống",
                },
                new() {
                    Id = Guid.NewGuid(),
                    AmenityName = "Bếp",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    CreatedBy = "Hệ thống",
                },
                new() {
                    Id = Guid.NewGuid(),
                    AmenityName = "Điều hòa",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    CreatedBy = "Hệ thống",
                },
                new() {
                    Id = Guid.NewGuid(),
                    AmenityName = "Máy giặt",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    CreatedBy = "Hệ thống",
                },
                ];
            return amenities;
        }
    }

}

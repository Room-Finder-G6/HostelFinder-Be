
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
            }
        }
        private IEnumerable<Service> GetServices()
        {
            List<Service> services = [
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "Ban công",
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
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
                    ServiceName = "Điều hòa",
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "Gác xếp",
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "Giường",
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "Nhà bếp",
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
                    ServiceName = "Tivi",
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "Tủ lạnh",
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
                    ServiceName = "Bình nóng lạnh",
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
                    ServiceName = "WC",
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "Tủ quần áo",
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "Máy giặt",
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                ];
            return services;
        }
    }

}

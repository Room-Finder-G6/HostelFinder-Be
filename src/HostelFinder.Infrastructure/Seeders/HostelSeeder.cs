
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
                        CreatedBy = "Hệ Thống",
                        CreatedOn = DateTime.Now,
                        FullName = "HostelFinder"
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
                if (!await dbContext.Memberships.AnyAsync())
                {
                    var memberships = GetMemberships();
                    await dbContext.Memberships.AddRangeAsync(memberships);
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
                    ChargingMethod = ChargingMethod.Free,
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "Bãi để xe",
                    ChargingMethod = ChargingMethod.Free,
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "Thang máy",
                    ChargingMethod = ChargingMethod.Free,
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "Internet",
                    ChargingMethod = ChargingMethod.PerPerson,
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "Giặt là",
                    ChargingMethod = ChargingMethod.PerUsageUnit,
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "Vệ sinh",
                    ChargingMethod = ChargingMethod.FlatFee,
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false
                },
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "Điện",
                    ChargingMethod = ChargingMethod.PerUsageUnit,
                    CreatedBy = "Hệ thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false
                },
                new () {
                    Id = Guid.NewGuid(),
                    ServiceName = "Nước",
                    ChargingMethod = ChargingMethod.PerUsageUnit,
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

        private IEnumerable<Membership> GetMemberships()
        {
            var memberships = new List<Membership>
            {
                new Membership
                {
                    Id = Guid.NewGuid(),
                    Name = "Hội viên Dùng Thử",
                    Description = "Gói hội viên dùng thử có hạn trong 7 ngày",
                    Price = 0,
                    Duration = 7,
                    Tag = "Dùng Thử",
                    CreatedBy = "Hệ Thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    MembershipServices = new List<MembershipServices>
                    {
                        new MembershipServices { Id = Guid.NewGuid(), ServiceName = "Số bài được đăng & Số lượt dẩy bài", MaxPostsAllowed = 5, MaxPushTopAllowed = 5, CreatedOn = DateTime.Now, CreatedBy = "Hệ Thống", IsDeleted = false },
                        new MembershipServices { Id = Guid.NewGuid(), ServiceName = "Quản lý nhà trọ", CreatedOn = DateTime.Now, CreatedBy = "Hệ Thống", IsDeleted = false }
                    }
                },
                new Membership
                {
                    Id = Guid.NewGuid(),
                    Name = "Hội viên Cơ bản",
                    Description = "Phù hợp với quy mô nhà trọ nhỏ",
                    Price = 100000,
                    Duration = 30,
                    Tag = "Đồng",
                    CreatedBy = "Hệ Thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    MembershipServices = new List<MembershipServices>
                    {
                        new MembershipServices { Id = Guid.NewGuid(), ServiceName = "Số bài được đăng & Số lượt dẩy bài", MaxPostsAllowed = 15, MaxPushTopAllowed = 15, CreatedOn = DateTime.Now, CreatedBy = "Hệ Thống", IsDeleted = false },
                        new MembershipServices { Id = Guid.NewGuid(), ServiceName = "Quản lý nhà trọ", CreatedOn = DateTime.Now, CreatedBy = "Hệ Thống", IsDeleted = false }
                    }
                },
                new Membership
                {
                    Id = Guid.NewGuid(),
                    Name = "Hội viên Tiêu chuẩn",
                    Description = "Phù hợp với quy mô và số lượng nhà trọ vừa từ 10 BDS",
                    Price = 150000,
                    Duration = 30,
                    Tag = "Bạc",
                    CreatedBy = "Hệ Thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    MembershipServices = new List<MembershipServices>
                    {
                        new MembershipServices { Id = Guid.NewGuid(), ServiceName = "Số bài được đăng & Số lượt dẩy bài", MaxPostsAllowed = 30, MaxPushTopAllowed = 30, CreatedOn = DateTime.Now, CreatedBy = "Hệ Thống", IsDeleted = false },
                        new MembershipServices { Id = Guid.NewGuid(), ServiceName = "Quản lý nhà trọ", CreatedOn = DateTime.Now, CreatedBy = "Hệ Thống", IsDeleted = false }
                    }
                },
                new Membership
                {
                    Id = Guid.NewGuid(),
                    Name = "Hội viên Cao cấp",
                    Description = "Phù hợp với quy mô và số lượng nhà trọ lớn",
                    Price = 200000,
                    Duration = 30,
                    Tag = "Vàng",
                    CreatedBy = "Hệ Thống",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    MembershipServices = new List<MembershipServices>
                    {
                        new MembershipServices { Id = Guid.NewGuid(), ServiceName = "Số bài được đăng & Số lượt dẩy bài", MaxPostsAllowed = 50, MaxPushTopAllowed = 50, CreatedOn = DateTime.Now, CreatedBy = "Hệ Thống", IsDeleted = false },
                        new MembershipServices { Id = Guid.NewGuid(), ServiceName = "Quản lý nhà trọ", CreatedOn = DateTime.Now, CreatedBy = "Hệ Thống", IsDeleted = false }
                    }
                }
            };

            return memberships;
        }
    }

}

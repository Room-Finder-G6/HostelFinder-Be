
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
            if(await dbContext.Database.CanConnectAsync())
            {
                if( await dbContext.Users.FirstOrDefaultAsync(x => x.Role == UserRole.Admin) == null)
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
            }
        }
    }
}

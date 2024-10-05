using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Services;
using HostelFinder.Infrastructure.Context;
using HostelFinder.Infrastructure.Repositories;
using HostelFinder.Infrastructure.Services;
using HostelFinder.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HostelFinder.Infrastructure;

public class ServiceRegistration
{
    public static void Configure(IServiceCollection service, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        service.AddDbContext<HostelFinderDbContext>(options =>
            options.UseSqlServer(connectionString));

        service.AddScoped<IHostelRepository, HostelRepository>();
        service.AddScoped<IUserRepository, UserRepository>();
        service.AddScoped<IRoomRepository, RoomRepository>();
        service.AddScoped<IWishlistRepository, WishlistRepository>();
        service.AddScoped<IEmailService, EmailService>();
        service.AddScoped<IAuthAccountService, AuthAccountService>();
        service.AddScoped<IServiceRepository, ServiceRepository>();
    }
}
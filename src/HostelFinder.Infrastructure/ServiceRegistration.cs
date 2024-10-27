using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Services;
using HostelFinder.Infrastructure.Context;
using HostelFinder.Infrastructure.Repositories;
using HostelFinder.Infrastructure.Seeders;
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
        service.AddScoped<IPostRepository, PostRepository>();
        service.AddScoped<IAmenityRepository, AmenityRepository>();
        service.AddScoped<IWishlistRepository, WishlistRepository>();
        service.AddScoped<IEmailService, EmailService>();
        service.AddScoped<IAuthAccountService, AuthAccountService>();
        service.AddScoped<IServiceRepository, ServiceRepository>();
        service.AddScoped<IServiceCostRepository, ServiceCostRepository>();
        service.AddScoped<IHostelSeeder, HostelSeeder>();
        service.AddScoped<IMembershipRepository, MembershipRepository>();
        service.AddScoped<IS3Service, S3Service>();
        service.AddScoped<IInVoiceRepository, InVoiceRepository>();
        service.AddScoped<IServiceCostRepository, ServiceCostRepository>();
    }
}
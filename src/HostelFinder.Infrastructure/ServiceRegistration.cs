using HostelFinder.Infrastructure.Context;
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
    }

}
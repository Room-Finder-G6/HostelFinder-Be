using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Mappings;
using HostelFinder.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HostelFinder.Application;

public class ServiceExtentions
{
    public static void ConfigureServices(IServiceCollection service, IConfiguration configuration)
    {
        service.AddScoped<IRoomService, RoomService>();
        service.AddAutoMapper(typeof(GeneralProfile));
    }
}
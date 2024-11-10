using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Exceptions;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories
{
    public class ServiceRepository : BaseGenericRepository<Service>, IServiceRepository
    {
        public ServiceRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Service?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Services.FindAsync(id);
        }

        public async Task<bool> CheckDuplicateServiceAsync(string serviceName)
        {
            return await _dbContext.Services.AnyAsync(s => s.ServiceName == serviceName);
        }

        public async Task<IEnumerable<Service>> GetAllServiceAsync()
        {
            return await _dbContext.Services
                            .Include(s => s.ServiceCosts)
                            .ToListAsync(); 
        }

        public async Task<Service> GetServiceByIdAsync(Guid serviceId)
        {
            var service = await _dbContext.Services
                .Include(s => s.ServiceCosts)
                .FirstOrDefaultAsync(s => s.Id == serviceId && !s.IsDeleted);
            if (service == null)
            {
                throw new NotFoundException("Không tìm thấy dịch vụ nào!");
            }
            return service;
        }

        public async Task<IEnumerable<Service>> GetServiceByRoomIdAsync(Guid roomId)
        {
            var services = await _dbContext.ServiceCosts
                .Where(sr => sr.RoomId == roomId && !sr.IsDeleted)
                .Include(sr => sr.Service)
                .Select(sr => sr.Service)
                .ToListAsync();
            if (!services.Any())
            {
                throw new NotFoundException("Phòng có dịch vụ nào trong phòng");
            }

            return services;
        }
    }
}

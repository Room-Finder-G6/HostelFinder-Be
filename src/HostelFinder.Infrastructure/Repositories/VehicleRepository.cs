using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;

namespace HostelFinder.Infrastructure.Repositories
{
    public class VehicleRepository : BaseGenericRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }
    }
}

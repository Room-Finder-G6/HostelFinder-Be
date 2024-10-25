using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories;

public class AmenityRepository : BaseGenericRepository<Amenity>, IAmenityRepository
{
    public AmenityRepository(HostelFinderDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> ExistsByNameAsync(string amenityName)
    {
        return await _dbContext.Amenities.AnyAsync(a => a.AmenityName == amenityName);
    }
}
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using HostelFinder.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories;

public class AmenityRepository : BaseGenericRepository<Amenity>, IAmenityRepository
{
    public AmenityRepository(HostelFinderDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Amenity> AddAmenityAsync(Amenity amenity)
    {
        try
        {
            await _dbContext.Amenities.AddAsync(amenity);
            await _dbContext.SaveChangesAsync();
            return amenity;
        }
        catch (DbUpdateException ex)
        {
            throw new RepositoryException("An error occurred while adding the amenity.", ex);
        }
    }

    public Task<List<Amenity>> GetAmenitiesAsync()
    {
        return _dbContext.Amenities.ToListAsync();
    }
}
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories;

public class HostelRepository : BaseGenericRepository<Hostel>, IHostelRepository
{
    public HostelRepository(HostelFinderDbContext dbContext) : base(dbContext)
    {
    }

    // Get all hostels
    public Task<IQueryable<Hostel>> GetAllHostelsAsync()
    {
        var hostels = _dbContext.Hostels.AsQueryable();
        return Task.FromResult(hostels);
    }

    // Get hostel by Id with all details (eager loading related entities)
    public async Task<Hostel> GetHostelWithDetailsAsync(Guid hostelId)
    {
        return await _dbContext.Hostels
            .Include(h => h.Services)
            .Include(h => h.Rooms)
            .Include(h => h.Reviews)
            .Include(h => h.Images)
            .FirstOrDefaultAsync(h => h.Id == hostelId && !h.IsDeleted);
    }

    // Get hostels by LandlordId
    public Task<IQueryable<Hostel>> GetHostelsByLandlordIdAsync(Guid landlordId)
    {
        var hostels = _dbContext.Hostels.Where(h => h.LandlordId == landlordId);
        return Task.FromResult(hostels);
    }

    // Search hostels by name
    public Task<IQueryable<Hostel>> GetHostelsByNameAsync(string search)
    {
        var hostels = _dbContext.Hostels.Where(h => h.HostelName.Contains(search));
        return Task.FromResult(hostels);
    }

    // Search hostels by address
    public Task<IQueryable<Hostel>> GetHostelsByAddressAsync(string address)
    {
        var hostels = _dbContext.Hostels.Where(h => h.Address.Contains(address));
        return Task.FromResult(hostels);
    }

    // Filter hostels by number of rooms
    public Task<IQueryable<Hostel>> GetHostelsByNumberOfRoomsAsync(int minRooms, int maxRooms)
    {
        var hostels = _dbContext.Hostels.Where(h => h.NumberOfRooms >= minRooms && h.NumberOfRooms <= maxRooms);
        return Task.FromResult(hostels);
    }

    // Filter hostels by rating
    public Task<IQueryable<Hostel>> GetHostelsByRatingAsync(float minRating, float maxRating)
    {
        var hostels = _dbContext.Hostels.Where(h => h.Rating >= minRating && h.Rating <= maxRating);
        return Task.FromResult(hostels);
    }

    // Add new hostel
    public Task<Hostel> AddHostelAsync(Hostel hostel)
    {
        return AddAsync(hostel);
    }

    // Update existing hostel
    public Task<Hostel> UpdateHostelAsync(Hostel hostel)
    {
        return UpdateAsync(hostel);
    }

    // Delete hostel by Id (soft delete)
    public Task<Hostel> DeleteHostelAsync(Guid hostelId)
    {
        return DeleteAsync(hostelId);
    }
}

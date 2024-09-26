using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories
{
    public interface IHostelRepository : IBaseGenericRepository<Hostel>
    {
        // Get all hostels
        Task<IQueryable<Hostel>> GetAllHostelsAsync();

        // Get hostel by Id with details (eager loading related entities)
        Task<Hostel> GetHostelWithDetailsAsync(Guid hostelId);

        // Get hostels by LandlordId
        Task<IQueryable<Hostel>> GetHostelsByLandlordIdAsync(Guid landlordId);

        // Search hostels by name
        Task<IQueryable<Hostel>> GetHostelsByNameAsync(string search);

        // Search hostels by address
        Task<IQueryable<Hostel>> GetHostelsByAddressAsync(string address);

        // Filter hostels by number of rooms
        Task<IQueryable<Hostel>> GetHostelsByNumberOfRoomsAsync(int minRooms, int maxRooms);

        // Filter hostels by rating
        Task<IQueryable<Hostel>> GetHostelsByRatingAsync(float minRating, float maxRating);

        // Add new hostel
        Task<Hostel> AddHostelAsync(Hostel hostel);

        // Update existing hostel
        Task<Hostel> UpdateHostelAsync(Hostel hostel);

        // Delete hostel by Id (soft delete)
        Task<Hostel> DeleteHostelAsync(Guid hostelId);
    }
}

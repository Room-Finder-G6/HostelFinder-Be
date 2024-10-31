using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories
{
    public interface IRoomRepository : IBaseGenericRepository<Room>
    {
        Task<bool> RoomExistsAsync(string roomName, Guid hostelId);
        Task<IEnumerable<Room>> ListAllWithDetailsAsync();
    }
}

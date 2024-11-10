using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories
{
    public interface IRoomRepository : IBaseGenericRepository<Room>
    {
        Task<bool> RoomExistsAsync(string roomName, Guid hostelId);
        Task<IEnumerable<Room>> ListAllWithDetailsAsync();
        Task<Room> GetRoomWithDetailsAndServiceCostsByIdAsync(Guid roomId);
        Task<List<Room>> GetRoomsByHostelIdAsync(Guid hostelId, int? floor);

        Task<Room> GetRoomByIdAsync(Guid roomId);


    }
}

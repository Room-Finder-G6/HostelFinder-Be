using HostelFinder.Application.Common;
using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories;

public interface IRoomRepository : IBaseGenericRepository<Room>
{
    Task<Room> GetAllRoomFeaturesByRoomId(Guid roomId);
    /*Task AddRoom(Room room);*/
}
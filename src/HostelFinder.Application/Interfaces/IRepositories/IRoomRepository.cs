using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories;

public interface IRoomRepository : IBaseGenericRepository<Room>
{
    Task<IQueryable<Room>> GetAllRoomFeaturesByRoomId(Guid roomId);
}
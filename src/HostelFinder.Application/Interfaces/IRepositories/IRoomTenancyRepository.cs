using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories
{
    public interface IRoomTenancyRepository : IBaseGenericRepository<RoomTenancy>
    {
        Task<int> CountCurrentTenantsAsync(Guid roomId);
        Task<List<RoomTenancy>> GetRoomTenacyByIdAsync(Guid roomId);

        /// <summary>
        /// lấy ra những người đang ở trong hợp đồng
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        Task<List<RoomTenancy>> GetTenacyCurrentlyByRoom(Guid roomId, DateTime startDate, DateTime? endDate);
    }
}

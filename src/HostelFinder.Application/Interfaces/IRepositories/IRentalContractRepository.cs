using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories
{
    public interface IRentalContractRepository : IBaseGenericRepository<RentalContract>
    {
        Task<RentalContract?> GetRoomRentalContrctByRoom(Guid roomId);
        /// <summary>
        /// Method trả về hợp đồng hợp lệ ngoài khoảng thời gian đang cho thuê hiện tại
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<RentalContract?> CheckExpiredContractAsync(Guid roomId,DateTime startDate, DateTime? endDate);
    }
}

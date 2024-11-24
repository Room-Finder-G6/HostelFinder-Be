using HostelFinder.Application.DTOs.RentalContract.Request;
using HostelFinder.Application.DTOs.Room.Responses;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface IRentalContractService
    {
        Task<Response<string>> CreateRentalContractAsync(AddRentalContractDto request);

        Task<RoomContractHistoryResponseDto> GetRoomContractHistoryLasest(Guid roomId);
    }
}

using HostelFinder.Application.DTOs.Hostel.Requests;
using HostelFinder.Application.DTOs.Hostel.Responses;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface IHostelService
    {
        Task<Response<HostelResponseDto>> AddHostelAsync(AddHostelRequestDto hostelDto);
        Task<Response<HostelResponseDto>> UpdateHostelAsync(UpdateHostelRequestDto hostelDto);
        Task<Response<bool>> DeleteHostelAsync(Guid hostelId);
        Task<IEnumerable<HostelResponseDto>> GetHostelsByLandlordIdAsync(Guid landlordId);
    }
}

using HostelFinder.Application.DTOs.Hostel.Requests;
using HostelFinder.Application.DTOs.Hostel.Responses;
using HostelFinder.Application.Wrappers;
using Microsoft.EntityFrameworkCore.Storage;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface IHostelService
    {
        Task<Response<HostelResponseDto>> AddHostelAsync(AddHostelRequestDto hostelDto, List<string> imageUrls);
        Task<Response<bool>> DeleteHostelAsync(Guid hostelId);
        Task<Response<List<ListHostelResponseDto>>> GetHostelsByUserIdAsync(Guid landlordId);
        Task<Response<HostelResponseDto>> GetHostelByIdAsync(Guid hostelId);
        Task<PagedResponse<List<ListHostelResponseDto>>> GetAllHostelAsync(GetAllHostelQuery request);
        Task<Response<HostelResponseDto>> UpdateHostelAsync(Guid hostelId, UpdateHostelRequestDto request, List<string> imageUrls);

    }
}
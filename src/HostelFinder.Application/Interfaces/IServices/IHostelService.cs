﻿using HostelFinder.Application.DTOs.Hostel.Requests;
using HostelFinder.Application.DTOs.Hostel.Responses;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface IHostelService
    {
        Task<Response<HostelResponseDto>> AddHostelAsync(AddHostelRequestDto hostelDto);
        Task<Response<HostelResponseDto>> UpdateHostelAsync(UpdateHostelRequestDto hostelDto);
        Task<Response<bool>> DeleteHostelAsync(Guid hostelId);
        Task<Response<List<HostelResponseDto>>> GetHostelsByLandlordIdAsync(Guid landlordId);
        Task<Response<HostelResponseDto>> GetHostelByIdAsync(Guid hostelId);
        Task<PagedResponse<List<ListHostelResponseDto>>> GetAllHostelAsync(GetAllHostelQuery request);
    }
}

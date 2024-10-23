using AutoMapper;
using HostelFinder.Application.DTOs.Hostel.Requests;
using HostelFinder.Application.DTOs.Hostel.Responses;
using HostelFinder.Application.Helpers;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.Application.Services
{
    public class HostelService : IHostelService
    {
        private readonly IHostelRepository _hostelRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public HostelService(IHostelRepository hostelRepository,IReviewRepository reviewRepository, IMapper mapper)
        {
            _hostelRepository = hostelRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public async Task<Response<HostelResponseDto>> AddHostelAsync(AddHostelRequestDto hostelDto)
        {
            var isDuplicate = await _hostelRepository.CheckDuplicateHostelAsync(
                hostelDto.HostelName,
                hostelDto.Address.Province,
                hostelDto.Address.District,
                hostelDto.Address.Commune,
                hostelDto.Address.DetailAddress
            );

            if (isDuplicate)
            {
                return new Response<HostelResponseDto>("Hostel đã tồn tại với cùng địa chỉ.");
            }

            var hostel = _mapper.Map<Hostel>(hostelDto);
            hostel.CreatedOn = DateTime.Now;
            hostel.CreatedBy = "System";
            try
            {
                await _hostelRepository.AddAsync(hostel);
                var hostelResponseDto = _mapper.Map<HostelResponseDto>(hostel);
                return new Response<HostelResponseDto> { Data = hostelResponseDto, Message = "Thêm trọ mới thành công." };
            }
            catch (Exception ex)
            {
                return new Response<HostelResponseDto>(message: ex.Message);
            }
        }

        public async Task<Response<HostelResponseDto>> UpdateHostelAsync(UpdateHostelRequestDto hostelDto)
        {
            var existingHostel = await _hostelRepository.GetByIdAsync(hostelDto.Id);
            if (existingHostel == null)
            {
                return new Response<HostelResponseDto>("Hostel not found");
            }

            try
            {
                _mapper.Map(hostelDto, existingHostel);
                existingHostel.LastModifiedOn = DateTime.Now;
                existingHostel.LastModifiedBy = "System";
                await _hostelRepository.UpdateAsync(existingHostel);

                var updatedHostelDto = _mapper.Map<HostelResponseDto>(existingHostel);
                return new Response<HostelResponseDto>(updatedHostelDto, "Update successful.");
            }
            catch (Exception ex)
            {
                return new Response<HostelResponseDto>(message: ex.Message);
            }
        }

        public async Task<Response<bool>> DeleteHostelAsync(Guid hostelId)
        {
            var hostel = await _hostelRepository.GetByIdAsync(hostelId);
            if (hostel == null)
            {
                return new Response<bool>(false, "Hostel not found");
            }

            try
            {
                await _hostelRepository.DeleteAsync(hostel.Id);
                return new Response<bool>(true, "Delete successful.");
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, message: ex.Message);
            }
        }

        public async Task<Response<List<HostelResponseDto>>> GetHostelsByLandlordIdAsync(Guid landlordId)
        {
            var hostels = await _hostelRepository.GetHostelsByLandlordIdAsync(landlordId);
            return _mapper.Map<Response<List<HostelResponseDto>>>(hostels);
        }

        public async Task<Response<HostelResponseDto>> GetHostelByIdAsync(Guid hostelId)
        {
            var hostel = await _hostelRepository.GetHostelByIdAsync(hostelId);
            if (hostel == null)
            {
                return new Response<HostelResponseDto>("Hostel not found.");
            }

            var hostelDto = _mapper.Map<HostelResponseDto>(hostel);

            var averageRating = await _reviewRepository.GetAverageRatingForHostelAsync(hostelId);
            hostelDto.Rating = averageRating;

            return new Response<HostelResponseDto>(hostelDto);
        }

        public async Task<PagedResponse<List<ListHostelResponseDto>>> GetAllHostelAsync(GetAllHostelQuery request)
        {
            try
            {
                var hostels = await _hostelRepository.GetAllMatchingAsync(request.SearchPhrase, request.PageSize, request.PageNumber, request.SortBy, request.SortDirection);

                var hostelDtos = _mapper.Map<List<ListHostelResponseDto>>(hostels.Data);

                var pagedResponse = PaginationHelper.CreatePagedResponse(hostelDtos, request.PageNumber, request.PageSize, hostels.TotalRecords);
                return pagedResponse;
            }
            catch (Exception ex)
            {
                return new PagedResponse<List<ListHostelResponseDto>> { Succeeded  = false, Errors = {ex.Message} };
            }
        }
    }

}

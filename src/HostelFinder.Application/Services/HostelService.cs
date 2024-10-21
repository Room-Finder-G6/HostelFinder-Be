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
        private readonly IMapper _mapper;

        public HostelService(IHostelRepository hostelRepository, IMapper mapper)
        {
            _hostelRepository = hostelRepository;
            _mapper = mapper;
        }

        public async Task<Response<HostelResponseDto>> AddHostelAsync(AddHostelRequestDto hostelDto)
        {
            var hostel = _mapper.Map<Hostel>(hostelDto);
            await _hostelRepository.AddAsync(hostel);
            var hostelResponseDto = _mapper.Map<HostelResponseDto>(hostel);
            return new Response<HostelResponseDto>(hostelResponseDto);
        }

        public async Task<Response<HostelResponseDto>> UpdateHostelAsync(UpdateHostelRequestDto hostelDto)
        {
            var existingHostel = await _hostelRepository.GetByIdAsync(hostelDto.Id);
            if (existingHostel == null)
            {
                return new Response<HostelResponseDto>("Hostel not found");
            }

            _mapper.Map(hostelDto, existingHostel);  // Update the entity with the new values
            await _hostelRepository.UpdateAsync(existingHostel);

            var updatedHostelDto = _mapper.Map<HostelResponseDto>(existingHostel);
            return new Response<HostelResponseDto>(updatedHostelDto);
        }

        public async Task<Response<bool>> DeleteHostelAsync(Guid hostelId)
        {
            var hostel = await _hostelRepository.GetByIdAsync(hostelId);
            if (hostel == null)
            {
                return new Response<bool>(false, "Hostel not found");
            }

            await _hostelRepository.DeleteAsync(hostel.Id);
            return new Response<bool>(true);
        }

        public async Task<IEnumerable<HostelResponseDto>> GetHostelsByLandlordIdAsync(Guid landlordId)
        {
            var hostels = await _hostelRepository.GetHostelsByLandlordIdAsync(landlordId);
            return _mapper.Map<IEnumerable<HostelResponseDto>>(hostels);
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

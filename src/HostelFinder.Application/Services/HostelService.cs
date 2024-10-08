using AutoMapper;
using HostelFinder.Application.DTOs.Hostel.Requests;
using HostelFinder.Application.DTOs.Hostel.Responses;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;

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

            _mapper.Map(hostelDto, existingHostel);
            existingHostel.LastModifiedOn = DateTime.Now;
            existingHostel.LastModifiedBy = "System";
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
    }

}

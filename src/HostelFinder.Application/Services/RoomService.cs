using AutoMapper;
using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.DTOs.Room.Responses;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<RoomResponseDto>>> GetAllAsync()
        {
            var rooms = await _roomRepository.ListAllWithDetailsAsync();
            var result = _mapper.Map<List<RoomResponseDto>>(rooms);
            return new Response<List<RoomResponseDto>>(result);
        }

        public async Task<Response<RoomResponseDto>> GetByIdAsync(Guid id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null)
                return new Response<RoomResponseDto>("Room not found.");

            var result = _mapper.Map<RoomResponseDto>(room);
            return new Response<RoomResponseDto>(result);
        }

        public async Task<Response<RoomResponseDto>> CreateAsync(AddRoomRequestDto roomDto)
        {
            bool roomExists = await _roomRepository.RoomExistsAsync(roomDto.RoomName, roomDto.HostelId);
            if (roomExists)
            {
                return new Response<RoomResponseDto>("A room with the same name already exists in this hostel.");
            }

            var room = _mapper.Map<Room>(roomDto);
            room.CreatedBy = "Hệ Thống";
            room.CreatedOn = DateTime.Now;

            if (roomDto.AddServiceCostDtos != null && roomDto.AddServiceCostDtos.Any())
            {
                room.ServiceCost = _mapper.Map<List<ServiceCost>>(roomDto.AddServiceCostDtos);
                foreach (var serviceCost in room.ServiceCost)
                {
                    serviceCost.Room = room;
                    serviceCost.CreatedOn = DateTime.UtcNow;
                    serviceCost.CreatedBy = room.CreatedBy;
                }
            }

            if (roomDto.RoomDetailRequestDto != null)
            {
                room.RoomDetails = _mapper.Map<RoomDetails>(roomDto.RoomDetailRequestDto);
                room.RoomDetails.Room = room;
                room.RoomDetails.CreatedOn = DateTime.UtcNow;
                room.RoomDetails.CreatedBy = room.CreatedBy;
            }

            room = await _roomRepository.AddAsync(room);

            var result = _mapper.Map<RoomResponseDto>(room);
            return new Response<RoomResponseDto>(result, "Room created successfully.");
        }

        public async Task<Response<RoomResponseDto>> UpdateAsync(Guid id, UpdateRoomRequestDto roomDto)
        {
            var room = await _roomRepository.GetRoomWithDetailsAndServiceCostsByIdAsync(id);
            if (room == null)
                return new Response<RoomResponseDto>("Room not found.");

            room.RoomName = roomDto.RoomName;
            room.Status = roomDto.Status;
            room.MonthlyRentCost = roomDto.MonthlyRentCost;
            room.RoomType = roomDto.RoomType;
            room.LastModifiedBy = room.LastModifiedBy;
            room.LastModifiedOn = DateTime.UtcNow;

            if (roomDto.UpdateRoomDetailsDto != null && room.RoomDetails != null)
            {
                _mapper.Map(roomDto.UpdateRoomDetailsDto, room.RoomDetails);
                room.RoomDetails.LastModifiedBy = room.LastModifiedBy;
                room.RoomDetails.LastModifiedOn = DateTime.UtcNow;
            }
            if (roomDto.UpdateServiceCostDtos != null)
            {
                var incomingServiceCosts = roomDto.UpdateServiceCostDtos
                    .Where(dto => dto.ServiceCostId != Guid.Empty)
                    .ToDictionary(sc => sc.ServiceCostId);

                foreach (var serviceCost in room.ServiceCost)
                {
                    if (incomingServiceCosts.TryGetValue(serviceCost.Id, out var updateDto))
                    {
                        _mapper.Map(updateDto, serviceCost);
                        serviceCost.LastModifiedBy = room.LastModifiedBy;
                        serviceCost.LastModifiedOn = DateTime.UtcNow;
                        incomingServiceCosts.Remove(serviceCost.Id);
                    }
                    else
                    {
                        Console.WriteLine("No matching ServiceCost ID found for: " + serviceCost.Id);
                    }
                }

            }
            room = await _roomRepository.UpdateAsync(room);
            var result = _mapper.Map<RoomResponseDto>(room);
            return new Response<RoomResponseDto>(result, "Room updated successfully.");
        }

        public async Task<Response<bool>> DeleteAsync(Guid id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null)
                return new Response<bool>("Room not found.");

            await _roomRepository.DeletePermanentAsync(id);
            return new Response<bool>(true, "Room deleted successfully.");
        }
    }
}

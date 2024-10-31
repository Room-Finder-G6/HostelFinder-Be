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

            // Add list of ServiceCost entities if provided
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

            // Add RoomDetail entity if provided
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
            // Fetch the room along with its details and service costs
            var room = await _roomRepository.GetRoomWithDetailsAndServiceCostsByIdAsync(id);
            if (room == null)
                return new Response<RoomResponseDto>("Room not found.");

            // Update room's main properties
            _mapper.Map(roomDto, room);

            // Update or add RoomDetail
            if (roomDto.UpdateRoomDetailsDto != null)
            {
                if (room.RoomDetails != null)
                {
                    // Update existing RoomDetail
                    _mapper.Map(roomDto.UpdateRoomDetailsDto, room.RoomDetails);
                    room.RoomDetails.LastModifiedBy = room.LastModifiedBy;
                    room.RoomDetails.LastModifiedOn = DateTime.UtcNow;
                }
                else
                {
                    // Add new RoomDetail
                    var newRoomDetail = _mapper.Map<RoomDetails>(roomDto.UpdateRoomDetailsDto);
                    newRoomDetail.RoomId = room.Id;
                    room.RoomDetails = newRoomDetail;
                    newRoomDetail.CreatedBy = room.CreatedBy;
                    newRoomDetail.CreatedOn = DateTime.UtcNow;
                }
            }

            // Update ServiceCosts
            if (roomDto.UpdateServiceCostDtos != null)
            {
                // Map ServiceCostDtos to a dictionary by Id for easier access
                var incomingServiceCosts = roomDto.UpdateServiceCostDtos.ToDictionary(sc => sc.ServiceCostId);

                // Update existing ServiceCosts or remove if not in incoming list
                //room.ServiceCost.RemoveAll(sc =>
                //{
                //    if (!incomingServiceCosts.TryGetValue(sc.Id, out var updateDto))
                //        return true; // Remove ServiceCost not in incoming list

                //    // Update existing ServiceCost
                //    _mapper.Map(updateDto, sc);
                //    sc.LastModifiedBy = room.LastModifiedBy;
                //    sc.LastModifiedOn = DateTime.UtcNow;
                //    incomingServiceCosts.Remove(sc.Id);
                //    return false;
                //});

                // Add new ServiceCosts from remaining items in the incoming dictionary
                foreach (var newServiceCostDto in incomingServiceCosts.Values)
                {
                    var newServiceCost = _mapper.Map<ServiceCost>(newServiceCostDto);
                    newServiceCost.RoomId = room.Id;
                    newServiceCost.CreatedBy = room.CreatedBy;
                    newServiceCost.CreatedOn = DateTime.UtcNow;
                    room.ServiceCost.Add(newServiceCost);
                }
            }

            // Save updates to the repository
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

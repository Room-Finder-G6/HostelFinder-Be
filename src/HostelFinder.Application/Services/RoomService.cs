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
            var rooms = await _roomRepository.ListAllAsync();
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
            var room = _mapper.Map<Room>(roomDto);
            room = await _roomRepository.AddAsync(room);

            var result = _mapper.Map<RoomResponseDto>(room);
            return new Response<RoomResponseDto>(result, "Room created successfully.");
        }

        public async Task<Response<RoomResponseDto>> UpdateAsync(Guid id, UpdateRoomRequestDto roomDto)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null)
                return new Response<RoomResponseDto>("Room not found.");

            _mapper.Map(roomDto, room);
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

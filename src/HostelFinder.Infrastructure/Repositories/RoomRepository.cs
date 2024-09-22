using HostelFinder.Application.Interfaces;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;

namespace HostelFinder.Infrastructure.Repositories;

public class RoomRepository : BaseGenericRepository<Room>, IRoomRepository
{
    public RoomRepository(HostelFinderDbContext dbContext) : base(dbContext)
    {
    }

    public Task<IQueryable<Room>> GetRoomsByHostelId(Guid hostelId)
    {
        var rooms = _dbContext.Rooms.Where(r => r.HostelId == hostelId);
        return Task.FromResult(rooms);
    }

    public Task<IQueryable<Room>> GetRoomsByHostelId(Guid hostelId, string? search)
    {
        var rooms = _dbContext.Rooms.Where(r => r.HostelId == hostelId && r.Title.Contains(search!));
        return Task.FromResult(rooms);
    }

    public Task<IQueryable<Room>> GetRoomByRoomId(Guid roomId)
    {
        var room = _dbContext.Rooms.Where(r => r.Id == roomId);
        return Task.FromResult(room);
    }

    public Task<IQueryable<Room>> GetRoomsByRoomType(string roomType)
    {
        var rooms = _dbContext.Rooms.Where(r => r.RoomType.TypeName == roomType);
        return Task.FromResult(rooms);
    }

    public Task<IQueryable<Room>> GetRoomsByPrice(decimal minPrice, decimal maxPrice)
    {
        var rooms = _dbContext.Rooms.Where(r => r.Price >= minPrice && r.Price <= maxPrice);
        return Task.FromResult(rooms);
    }

    public Task<IQueryable<Room>> GetRoomsByTitle(string title)
    {
        var rooms = _dbContext.Rooms.Where(r => r.Title == title);
        return Task.FromResult(rooms);
    }

    public Task<Room> AddRoomAsync(Room room)
    {
        return AddAsync(room);
    }

    public Task<Room> UpdateRoomAsync(Room room)
    {
        return UpdateAsync(room);
    }

    public Task<Room> DeleteRoomAsync(Guid roomId)
    {
        return DeleteAsync(roomId);
    }
}
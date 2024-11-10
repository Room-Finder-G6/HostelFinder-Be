using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.InkML;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Exceptions;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories
{
    public class RoomRepository : BaseGenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Room> GetRoomWithDetailsAndServiceCostsByIdAsync(Guid roomId)
        {
            var room = await _dbContext.Rooms
                    .Include(r => r.RoomDetails)
                    .Include(r => r.ServiceCosts)
                    .FirstOrDefaultAsync(r => r.Id == roomId && !r.IsDeleted);
            if (room == null)
            {
                throw new NotFoundException("Không tìm thấy phòng trọ nào!");
            }
            return room;
        }

        public async Task<IEnumerable<Room>> ListAllWithDetailsAsync()
        {
            return await _dbContext.Rooms
                          .Include(r => r.ServiceCosts)
                          .Include(r => r.RoomDetails)
                          .ToListAsync();
        }

        public async Task<List<Room>> GetRoomsByHostelIdAsync(Guid hostelId, int? floor)
        {
            IQueryable<Room> query =  _dbContext.Rooms
                            .AsNoTracking()
                            .Include(r => r.Hostel)
                            .Include(r => r.ServiceCosts)
                                .ThenInclude(sc => sc.Service)
                            .Include(r => r.Invoices)
                            .Where(r => r.HostelId == hostelId && !r.IsDeleted);
            if (floor.HasValue)
            {
                query = query.Where(r => r.Floor == floor); 
            }

            return await query.ToListAsync();

        }

        public async Task<bool> RoomExistsAsync(string roomName, Guid hostelId)
        {
            return await _dbContext.Rooms.AnyAsync(r => r.RoomName == roomName && r.HostelId == hostelId);
        }

        public async Task<Room> GetRoomByIdAsync(Guid roomId)
        {
            var room = await _dbContext.Rooms
                .Include(r => r.ServiceCosts)
                .ThenInclude(sc => sc.Service)
                .Include(r => r.Invoices)
                .Include(r => r.Hostel)
                .FirstOrDefaultAsync(r => r.Id == roomId && !r.IsDeleted);

            if(room == null)
            {
                throw new NotFoundException("Không tìm thấy phòng trọ nào!");
            }

            return room;
        }
    }
}

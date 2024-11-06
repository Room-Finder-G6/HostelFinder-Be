﻿using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.InkML;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
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
            return await _dbContext.Rooms
                    .Include(r => r.RoomDetails)  
                    .Include(r => r.ServiceCosts)  
                    .FirstOrDefaultAsync(r => r.Id == roomId);  
        }

        public async Task<IEnumerable<Room>> ListAllWithDetailsAsync()
        {
            return await _dbContext.Rooms
                          .Include(r => r.ServiceCosts)
                          .Include(r => r.RoomDetails)   
                          .ToListAsync();
        }

        public async Task<List<Room>> GetRoomsByHostelIdAsync(Guid hostelId)
        {
            return await _dbContext.Rooms
                .Include(x => x.Hostel)
                .Include(r => r.RoomDetails)
                .Include(r => r.ServiceCosts)
                .Where(r => r.HostelId == hostelId)
                .ToListAsync();
        }

        public async Task<bool> RoomExistsAsync(string roomName, Guid hostelId)
        {
            return await _dbContext.Rooms.AnyAsync(r => r.RoomName == roomName && r.HostelId == hostelId);
        }
    }
}

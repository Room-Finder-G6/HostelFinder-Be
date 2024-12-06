using System.Linq.Expressions;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Common.Constants;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories;

public class MaintenanceRecordRepository : BaseGenericRepository<MaintenanceRecord>, IMaintenanceRecordRepository
{
    public MaintenanceRecordRepository(HostelFinderDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(IEnumerable<MaintenanceRecord> Data, int TotalRecords)> GetAllMatchingInMaintenanceRecordAsync(Guid hostelId, string? searchPhrase, int pageSize, int pageNumber, string? sortBy,
        SortDirection? sortDirection)
    {
        var searchPhraseLower = searchPhrase?.ToLower();
        var baseQuery = _dbContext.MaintenanceRecords.Include(m => m.Room).Include(x => x.Hostel)
            .Where(x =>x.HostelId == hostelId  && searchPhraseLower == null || (x.Room.RoomName.ToLower().Contains(searchPhraseLower)
                                                      || x.Hostel.HostelName.ToLower().Contains(searchPhraseLower)
                                                      || x.Title.ToLower().Contains(searchPhraseLower)
                                                      || x.Description.ToLower().Contains(searchPhraseLower))
                                                      || x.Cost.ToString().Contains(searchPhraseLower)
                                                      && !x.IsDeleted);

        var totalRecords = await baseQuery.CountAsync();

        if (sortBy != null)
        {
            var columnsSelector = new Dictionary<string, Expression<Func<MaintenanceRecord, object>>>
            {
                { nameof(MaintenanceRecord.Room.RoomName), x => x.Room.RoomName },
                { nameof(MaintenanceRecord.Hostel.HostelName), x => x.Hostel.HostelName },
                { nameof(MaintenanceRecord.Title), x => x.Title },
                { nameof(MaintenanceRecord.Description), x => x.Description },
                { nameof(MaintenanceRecord.Cost), x => x.Cost }
            };

            var selectedColumn = columnsSelector[sortBy];

            baseQuery = sortDirection == SortDirection.Ascending
                ? baseQuery.OrderBy(selectedColumn)
                : baseQuery.OrderByDescending(selectedColumn);
        }

        var maintenanceRecords = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (Data: maintenanceRecords, TotalRecords: totalRecords);
    }
}
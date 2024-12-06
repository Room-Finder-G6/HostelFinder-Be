using HostelFinder.Application.Common;
using HostelFinder.Domain.Common.Constants;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories;

public interface IMaintenanceRecordRepository : IBaseGenericRepository<MaintenanceRecord>
{
    Task<(IEnumerable<MaintenanceRecord> Data, int TotalRecords)> GetAllMatchingInMaintenanceRecordAsync(Guid hostelId,string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection? sortDirection);
}
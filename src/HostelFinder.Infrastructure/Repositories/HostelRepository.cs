using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Common.Constants;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories;

public class HostelRepository : BaseGenericRepository<Hostel>, IHostelRepository
{
    public HostelRepository(HostelFinderDbContext dbContext) : base(dbContext)
    {
    }
    public async Task<IEnumerable<Hostel>> GetHostelsByLandlordIdAsync(Guid landlordId)
    {
        return await _dbContext.Hostels.Where(h => h.LandlordId == landlordId).ToListAsync();
    }

    public async Task<(IEnumerable<Hostel> Data, int TotalRecords)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection)
    {
        var searchPhraseLower = searchPhrase?.ToLower();


        var baseQuery = _dbContext.Hostels.Include(h => h.Landlord)
            .Where(x => searchPhraseLower == null || (x.HostelName.ToLower().Contains(searchPhraseLower)
            || x.Landlord.Username.ToLower().Contains(searchPhraseLower)));

        var totalRecords = await baseQuery.CountAsync();

        if(sortBy != null)
        {
            var columnsSelector = new Dictionary<string, Expression<Func<Hostel, object>>>
            {
                {nameof(Hostel.HostelName), x => x.HostelName},
                {nameof(Hostel.Rating), x => x.Rating }
            };

            var selectedColumn = columnsSelector[sortBy];

            baseQuery = sortDirection == SortDirection.Ascending
                ? baseQuery.OrderBy(selectedColumn)
                : baseQuery.OrderByDescending(selectedColumn);
        }
        var hostels = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (Data : hostels, TotalRecords : totalRecords);
    }
}

using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Common.Constants;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace HostelFinder.Infrastructure.Repositories;

public class HostelRepository : BaseGenericRepository<Hostel>, IHostelRepository
{
    public HostelRepository(HostelFinderDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> CheckDuplicateHostelAsync(string hostelName, string province, string district,
        string commune, string detailAddress)
    {
        return await _dbContext.Hostels.AnyAsync(h =>
            h.HostelName == hostelName
            && h.Address.Province == province
            && h.Address.District == district
            && h.Address.Commune == commune
            && h.Address.DetailAddress == detailAddress
        );
    }

    public async Task<IEnumerable<Hostel>> GetHostelsByUserIdAsync(Guid landlordId)
    {
        return await _dbContext.Hostels.Where(h => h.LandlordId == landlordId && !h.IsDeleted)
            .Include(a => a.Address)
            .Include(i => i.Images)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Hostel> Data, int TotalRecords)> GetAllMatchingAsync(string? searchPhrase,
        int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection)
    {
        var searchPhraseLower = searchPhrase?.ToLower();
        var baseQuery = _dbContext.Hostels.Include(h => h.Landlord)
            .Where(x => searchPhraseLower == null || (x.HostelName.ToLower().Contains(searchPhraseLower)
                                                      || x.Landlord.Username.ToLower().Contains(searchPhraseLower)));

        var totalRecords = await baseQuery.CountAsync();

        if (sortBy != null)
        {
            var columnsSelector = new Dictionary<string, Expression<Func<Hostel, object>>>
            {
                { nameof(Hostel.HostelName), x => x.HostelName }
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

        return (Data: hostels, TotalRecords: totalRecords);
    }

    public Task<Hostel> GetHostelByIdAndUserIdAsync(Guid hostelId, Guid userId)
    {
        return Task.FromResult(_dbContext.Hostels.Include(h => h.Address)
            .FirstOrDefault(h => h.Id == hostelId && h.LandlordId == userId));
    }

    public async Task<Hostel> GetHostelWithReviewsByPostIdAsync(Guid postId)
    {
        var post = await _dbContext.Posts
            .Include(p => p.Hostel)
            .FirstOrDefaultAsync(p => p.Id == postId);

        return post?.Hostel;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task<(IEnumerable<Hostel> Data, int TotalRecords)> GetAllMatchingInLandLordAsync(Guid landlordId, string? searchPhrase, int? pageSize, int? pageNumber, string? sortBy,
        SortDirection? sortDirection)
    {
        if (pageNumber == null)
        {
            pageNumber = 1;
        }
        if(pageSize == null)
        {
            pageSize = 10;
        }

        if (sortDirection == null)
        {
            sortDirection = SortDirection.Ascending;
        }
        
        var searchPhraseLower = searchPhrase?.ToLower();
        var baseQuery = _dbContext.Hostels.Include(h => h.Landlord)
            .Include(h => h.Address)
            .Include(h => h.Images)
             .Where(x => x.LandlordId == landlordId);
        baseQuery = baseQuery.Where(x => searchPhraseLower == null || (x.HostelName.ToLower().Contains(searchPhraseLower)
                                                                      || x.Landlord.Username.ToLower().Contains(searchPhraseLower)));

        var totalRecords = await baseQuery.CountAsync();
        //default sort by create on 
        if (sortBy == null)
        {
            baseQuery = baseQuery.OrderByDescending(x => x.CreatedOn);
        }
        

        if (sortBy != null)
        {
            var columnsSelector = new Dictionary<string, Expression<Func<Hostel, object>>>
            {
                { nameof(Hostel.HostelName), x => x.HostelName },
                { nameof(Hostel.Size), x => x.Size},
                { nameof(Hostel.NumberOfRooms), x => x.NumberOfRooms},
                { nameof(Hostel.CreatedOn), x => x.CreatedOn},
                { nameof(Hostel.LastModifiedOn), x => x.LastModifiedOn}
            };

            var selectedColumn = columnsSelector[sortBy];

            baseQuery = sortDirection == SortDirection.Ascending
                ? baseQuery.OrderBy(selectedColumn)
                : baseQuery.OrderByDescending(selectedColumn);
        }

        
        var hostels = await baseQuery
            .Skip(pageSize.Value * (pageNumber.Value - 1))
            .Take(pageSize.Value)
            .ToListAsync();

        return (Data: hostels, TotalRecords: totalRecords);
    }

    public async Task<Hostel?> GetHostelByIdAsync(Guid hostelId)
    {
        return await _dbContext.Hostels
            .Include(h => h.Address)
            .Include(i => i.Images)
            .FirstOrDefaultAsync(h => h.Id == hostelId);
    }
}
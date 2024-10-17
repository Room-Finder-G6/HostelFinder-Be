using DocumentFormat.OpenXml.InkML;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HostelFinder.Infrastructure.Repositories;

public class HostelRepository : BaseGenericRepository<Hostel>, IHostelRepository
{
    public HostelRepository(HostelFinderDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> CheckDuplicateHostelAsync(string hostelName, string province, string district, string commune, string detailAddress)
    {
        return await _dbContext.Hostels.AnyAsync(h =>
            h.HostelName == hostelName
            && h.Address.Province == province
            && h.Address.District == district
            && h.Address.commune == commune
            && h.Address.DetailAddress == detailAddress
        );
    }
    public async Task<IEnumerable<Hostel>> GetHostelsByLandlordIdAsync(Guid landlordId)
    {
        return await _dbContext.Hostels.Where(h => h.LandlordId == landlordId).ToListAsync();
    }

    public async Task<Hostel> GetHostelByPostIdAsync(Guid postId)
    {
        return await _dbContext.Posts
            .Include(p => p.Hostel)
                .ThenInclude(h => h.Reviews)
            .Where(p => p.Id == postId)
            .Select(p => p.Hostel)
            .FirstOrDefaultAsync();
    }

    public async Task<Hostel> GetHostelWithReviewsByPostIdAsync(Guid postId)
    {
        var post = await _dbContext.Posts
            .Include(p => p.Hostel)
                .ThenInclude(h => h.Reviews)  
            .FirstOrDefaultAsync(p => p.Id == postId);

        return post?.Hostel;  
    }
}

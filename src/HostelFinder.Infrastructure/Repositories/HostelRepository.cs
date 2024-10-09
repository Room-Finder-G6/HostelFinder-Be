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
}

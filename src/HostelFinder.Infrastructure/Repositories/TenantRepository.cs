using HostelFinder.Application.DTOs.Tenancies.Responses;
using HostelFinder.Application.Helpers;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories
{
    public class TenantRepository : BaseGenericRepository<Tenant>, ITenantRepository
    {
        public TenantRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Tenant> GetByIdentityCardNumber(string identityCardNumber)
        {
            var tenant = await _dbContext.Tenants.FirstOrDefaultAsync(x => x.IdentityCardNumber == identityCardNumber && !x.IsDeleted);
            return tenant;
        }

        public async Task<PagedResponse<List<InformationTenanciesResponseDto>>> GetTenantsByHostelAsync(Guid hostelId, string? roomName, int pageNumber, int pageSize)
        {
            var query = _dbContext.RoomTenancies
                .AsNoTracking()
                .Include(rt => rt.Room)
                .Include(rt => rt.Tenant)
                .Where(rt => rt.Room.HostelId == hostelId);

            if (!string.IsNullOrEmpty(roomName))
            {
                query = query.Where(rt => rt.Room.RoomName.Contains(roomName));
            }

            query = query.OrderBy(rt => rt.CreatedOn);

            var tenants = await query
                .Select(rt => new InformationTenanciesResponseDto
                {
                    TenancyId = rt.TenantId,
                    HostelId = hostelId,
                    RoomId = rt.RoomId,
                    RoomName = rt.Room.RoomName,
                    AvatarUrl = rt.Tenant.AvatarUrl,
                    FullName = rt.Tenant.FullName,
                    Email = rt.Tenant.Email,
                    Phone = rt.Tenant.Phone,
                    MoveInDate = rt.MoveInDate,
                    Status = rt.MoveOutDate.HasValue && rt.MoveOutDate < DateTime.Now ? "Đã rời phòng" : "Đang thuê"
                })
                .ToListAsync();

            var totalRecords = tenants.Count();
            var pagedData = tenants.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var pagedResponse = PaginationHelper.CreatePagedResponse(pagedData, pageNumber, pageSize, totalRecords);

            return pagedResponse;
        }
    }
}

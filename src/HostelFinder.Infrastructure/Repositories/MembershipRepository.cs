using DocumentFormat.OpenXml.InkML;
using HostelFinder.Application.DTOs.Membership.Responses;
using HostelFinder.Application.DTOs.MembershipService.Requests;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using HostelFinder.Infrastructure.Common;
using HostelFinder.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace HostelFinder.Infrastructure.Repositories
{
    public class MembershipRepository : BaseGenericRepository<Membership>, IMembershipRepository
    {
        public MembershipRepository(HostelFinderDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Membership>> GetAllMembershipWithMembershipService()
        {
            return await _dbContext.Memberships.
                    Include(mb => mb.Membership_Services).ToListAsync();
        }

        public async Task AddMembershipWithServicesAsync(Membership membership, List<AddMembershipServiceReqDto> membershipServices)
        {
            membership.Membership_Services = membershipServices
                .Select(ms => new Membership_Services
                {
                    Service_Name = ms.ServiceName,
                    Membership = membership,
                    CreatedOn = DateTime.Now,
                    CreatedBy = "System"
                }).ToList();

            await _dbContext.Memberships.AddAsync(membership);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> CheckDuplicateMembershipAsync(string name, string description)
        {
            return await _dbContext.Memberships
                .AnyAsync(mb => mb.Name == name && mb.Description == description);
        }

        public async Task<Membership> GetMembershipWithServicesAsync(Guid id)
        {
            var membership = await _dbContext.Memberships
                .Include(m => m.Membership_Services) 
                .FirstOrDefaultAsync(m => m.Id == id);
            return membership;
        }

        public void Update(Membership_Services entity)
        {
            _dbContext.Set<Membership_Services>().Update(entity);
        }

        public async Task UpdateAsync(Membership entity)
        {
            _dbContext.Set<Membership>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task Add(Membership_Services entity)
        {
            await _dbContext.Set<Membership_Services>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Membership_Services>> GetMembershipServicesByMembershipIdAsync(Guid membershipId)
        {
            return await _dbContext.MembershipServices
                .Where(ms => ms.MembershipId == membershipId)
                .ToListAsync();
        }
    }
}

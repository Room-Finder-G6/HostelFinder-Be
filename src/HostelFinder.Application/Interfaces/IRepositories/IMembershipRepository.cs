using HostelFinder.Application.Common;
using HostelFinder.Application.DTOs.MembershipService.Requests;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories
{
    public interface IMembershipRepository : IBaseGenericRepository<Membership>
    {
        Task Add(Membership_Services entity);
        void Update(Membership_Services entity);
        Task<IEnumerable<Membership>> GetAllMembershipWithMembershipService();
        Task<Membership> GetMembershipWithServicesAsync(Guid id);
        Task<bool> CheckDuplicateMembershipAsync(string name, string description);
        Task AddMembershipWithServicesAsync(Membership membership, List<AddMembershipServiceReqDto> membershipServices);
        Task<List<Membership_Services>> GetMembershipServicesByMembershipIdAsync(Guid membershipId);
    }
}

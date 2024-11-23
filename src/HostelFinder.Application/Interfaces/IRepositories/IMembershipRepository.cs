using HostelFinder.Application.Common;
using HostelFinder.Application.DTOs.MembershipService.Requests;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories
{
    public interface IMembershipRepository : IBaseGenericRepository<Membership>
    {
        Task Add(MembershipServices entity);
        void Update(MembershipServices entity);
        Task<IEnumerable<Membership>> GetAllMembershipWithMembershipService();
        Task<Membership> GetMembershipWithServicesAsync(Guid id);
        Task<bool> CheckDuplicateMembershipAsync(string name, string description);
        Task AddMembershipWithServicesAsync(Membership membership, List<AddMembershipServiceReqDto> membershipServices);
        Task<List<MembershipServices?>> GetMembershipServicesByMembershipIdAsync(Guid membershipId);
        Task<MembershipServices?> GetMembershipServiceWithPostsAsync(Guid membershipServiceId);
        Task<List<MembershipServices?>> GetAllMembershipServices();
    }
}

using HostelFinder.Application.DTOs.Membership.Requests;
using HostelFinder.Application.DTOs.Membership.Responses;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface IMembershipService
    {
        Task<Response<List<MembershipResponseDto>>> GetAllMembershipWithMembershipService();
        Task<Response<MembershipResponseDto>> AddMembershipAsync(AddMembershipRequestDto membershipDto);
        Task<Response<MembershipResponseDto>> EditMembershipAsync(Guid id, UpdateMembershipRequestDto membershipDto);
        Task<Response<string>> DeleteMembershipAsync(Guid id);
    }
}

using HostelFinder.Application.DTOs.MembershipService.Responses;

namespace HostelFinder.Application.DTOs.Membership.Responses
{
    public class MembershipResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public List<MembershipServiceResponseDto> MembershipServices { get; internal set; }
    }
}

using HostelFinder.Application.DTOs.MembershipService.Responses;

namespace HostelFinder.Application.DTOs.Membership.Requests
{
    public class AddMembershipRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public List<MembershipServiceResponseDto> MembershipServices { get; set; }
    }
}

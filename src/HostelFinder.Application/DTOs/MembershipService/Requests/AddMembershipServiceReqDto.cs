namespace HostelFinder.Application.DTOs.MembershipService.Requests
{
    public class AddMembershipServiceReqDto
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public int MaxPostsAllowed { get; set; }
    }
}

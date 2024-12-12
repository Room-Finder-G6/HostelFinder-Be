namespace HostelFinder.Application.DTOs.MembershipService.Responses
{
    public class MembershipServiceResponseDto
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public int MaxPostsAllowed { get; set; }
        public int MaxPushTopAllowed {  get; set; }
    }
}

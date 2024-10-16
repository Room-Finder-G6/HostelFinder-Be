namespace HostelFinder.Application.DTOs.Review.Request
{
    public class AddReviewRequestDto
    {
        public string Comment { get; set; }
        public int Rating { get; set; }
        public Guid UserId { get; set; }
        public Guid HostelId { get; set; }
    }
}

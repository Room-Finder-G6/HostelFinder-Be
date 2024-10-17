namespace HostelFinder.Application.DTOs.Review.Response
{
    public class ReviewResponseDto
    {
        public Guid Id { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
        public Guid UserId { get; set; }
        public Guid HostelId { get; set; }
    }
}

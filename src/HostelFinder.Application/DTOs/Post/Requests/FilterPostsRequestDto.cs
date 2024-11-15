using HostelFinder.Domain.Enums;

namespace HostelFinder.Application.DTOs.Post.Requests
{
    public class FilterPostsRequestDto
    {
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Commune { get; set; }
        public float? Size { get; set; }
        public decimal? minPrice { get; set; }
        public decimal? maxPrice { get; set; }
        public RoomType? RoomType { get; set; }
    }
}

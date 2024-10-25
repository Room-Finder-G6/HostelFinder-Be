using HostelFinder.Application.DTOs.Image.Responses;

namespace HostelFinder.Application.DTOs.Hostel.Responses
{
    public class ListHostelResponseDto
    {
        public string LandlordUserName { get; set; }
        public string HostelName { get; set; }
        public string? Description { get; set; }
        public float? Size { get; set; }
        public int NumberOfRooms { get; set; }
        public List<ImageResponseDto> Image { get; set; }
    }
}

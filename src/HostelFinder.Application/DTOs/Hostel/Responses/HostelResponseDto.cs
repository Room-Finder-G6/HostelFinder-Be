using HostelFinder.Application.DTOs.Address;
using HostelFinder.Application.DTOs.Image.Responses;

namespace HostelFinder.Application.DTOs.Hostel.Responses
{
    public class HostelResponseDto
    {
        public Guid Id { get; set; }
        public string HostelName { get; set; }
        public string? Description { get; set; }
        public AddressDto Address { get; set; }
        public int Size { get; set; }
        public int NumberOfRooms { get; set; }
        public List<ImageResponseDto> Image { get; set; }
        public string Coordinates { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}

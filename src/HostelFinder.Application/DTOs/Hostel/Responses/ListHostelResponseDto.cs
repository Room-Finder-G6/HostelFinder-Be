using HostelFinder.Application.DTOs.Address;
using HostelFinder.Application.DTOs.Image.Responses;

namespace HostelFinder.Application.DTOs.Hostel.Responses
{
    public class ListHostelResponseDto
    {
        public Guid Id { get; set; }
        public string LandlordUserName { get; set; }
        public string HostelName { get; set; }
        public AddressDto Address { get; set; }
        public float? Size { get; set; }
        public int NumberOfRooms { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public List<ImageResponseDto> Image { get; set; }
    }
}

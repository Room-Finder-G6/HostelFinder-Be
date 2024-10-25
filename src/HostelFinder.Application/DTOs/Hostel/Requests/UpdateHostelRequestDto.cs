using HostelFinder.Application.DTOs.Address;

namespace HostelFinder.Application.DTOs.Hostel.Requests
{
    public class UpdateHostelRequestDto
    {
        public string HostelName { get; set; }
        public string? Description { get; set; }
        public AddressDto Address { get; set; }
        public float? Size { get; set; }
        public int NumberOfRooms { get; set; }
        public string? Coordinates { get; set; }
    }
}

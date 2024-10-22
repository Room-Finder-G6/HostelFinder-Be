using HostelFinder.Application.DTOs.Address;
using HostelFinder.Application.DTOs.Review.Response;

namespace HostelFinder.Application.DTOs.Hostel.Responses
{
    public class HostelResponseDto
    {
        public Guid Id { get; set; }
        public string HostelName { get; set; }
        public string? Description { get; set; }
        public AddressDto Address { get; set; }
        public int NumberOfRooms { get; set; }
        public float Rating { get; set; }
        public string Image { get; set; }
        public string Coordinates { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        //public List<ReviewResponseDto> Reviews { get; set; }
    }
}

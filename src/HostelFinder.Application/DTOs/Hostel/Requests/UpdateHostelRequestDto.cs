using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.DTOs.ServiceCost.Request;

namespace HostelFinder.Application.DTOs.Hostel.Requests
{
    public class UpdateHostelRequestDto
    {
        public Guid Id { get; set; }
        public Guid? LandlordId { get; set; }
        public string HostelName { get; set; }
        public string? Description { get; set; }
        public string Address { get; set; }
        public float? Size { get; set; }
        public int NumberOfRooms { get; set; }
        public string? Coordinates { get; set; }
        public float Rating { get; set; }
    }
}

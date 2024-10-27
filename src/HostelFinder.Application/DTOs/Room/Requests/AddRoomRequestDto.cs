using HostelFinder.Application.DTOs.Amenity.Request;
using HostelFinder.Application.DTOs.RoomDetails.Request;
using HostelFinder.Application.DTOs.ServiceCost.Request;
using HostelFinder.Domain.Enums;

namespace HostelFinder.Application.DTOs.Room.Requests
{
    public class AddRoomRequestDto
    {
        public Guid HostelId { get; set; }
        public string RoomName { get; set; }
        public bool Status { get; set; }
        public decimal MonthlyRentCost { get; set; }
        public RoomType RoomType { get; set; }
    }
}

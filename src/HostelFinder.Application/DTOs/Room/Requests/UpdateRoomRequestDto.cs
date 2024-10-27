using HostelFinder.Domain.Enums;

namespace HostelFinder.Application.DTOs.Room.Requests
{
    public class UpdateRoomRequestDto
    {
        public string RoomName { get; set; }
        public bool Status { get; set; }
        public decimal MonthlyRentCost { get; set; }
        public RoomType RoomType { get; set; }
    }
}

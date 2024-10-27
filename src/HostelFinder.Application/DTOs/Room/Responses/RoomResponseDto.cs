using HostelFinder.Domain.Enums;

namespace HostelFinder.Application.DTOs.Room.Responses
{
    public class RoomResponseDto
    {
        public Guid Id { get; set; }
        public Guid HostelId { get; set; }
        public string RoomName { get; set; }
        public bool Status { get; set; }
        public decimal MonthlyRentCost { get; set; }
        public RoomType RoomType { get; set; }
    }
}

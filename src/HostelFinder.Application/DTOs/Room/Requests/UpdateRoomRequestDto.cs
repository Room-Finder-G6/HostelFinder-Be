using HostelFinder.Application.DTOs.RoomDetails.Request;
using HostelFinder.Application.DTOs.ServiceCost.Request;
using HostelFinder.Domain.Enums;

namespace HostelFinder.Application.DTOs.Room.Requests
{
    public class UpdateRoomRequestDto
    {
        public string RoomName { get; set; }
        public bool IsAvailable { get; set; }
        public decimal MonthlyRentCost { get; set; }
        public RoomType RoomType { get; set; }
        public List<UpdateServiceCostDto> UpdateServiceCostDtos { get; set; }
        public UpdateRoomDetailsDto UpdateRoomDetailsDto { get; set; }
    }
}

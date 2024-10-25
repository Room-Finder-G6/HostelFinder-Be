using HostelFinder.Application.DTOs.Amenity.Request;
using HostelFinder.Application.DTOs.RoomDetails.Request;
using HostelFinder.Application.DTOs.ServiceCost.Request;

namespace HostelFinder.Application.DTOs.Room.Requests
{
    public class AddRoomRequestDto
    {
        public string RoomName { get; set; }
        public Guid HostelId { get; set; }
        public decimal MonthlyRentCost { get; set; }
        public bool Status { get; set; }
        public AddRoomDetailsDto addRoomDetails { get; set; }
        public List<AddAmenityDto> addAmenityDtos { get; set; }
        public List<AddServiceCostDto> addServiceCostDtos { get; set; }
    }
}

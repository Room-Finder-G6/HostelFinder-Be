using HostelFinder.Application.DTOs.Address;
using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Application.DTOs.Hostel.Requests
{
    public class AddHostelRequestDto
    {
        [Required]
        public Guid? LandlordId { get; set; }
        [Required]
        public string HostelName { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public AddressDto Address { get; set; }
        [Range(0, int.MaxValue)]
        public float? Size { get; set; }
        [Range(0, int.MaxValue)]
        public int NumberOfRooms { get; set; }
        [Required]
        public string? Coordinates { get; set; }
        [Range(1, 5)]
        public float Rating { get; set; }
    }
}

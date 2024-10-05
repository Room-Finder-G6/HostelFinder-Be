using HostelFinder.Application.DTOs.RoomAmenities.Response;
using HostelFinder.Application.DTOs.RoomDetails.Response;
using HostelFinder.Application.DTOs.ServiceCost.Responses;
using HostelFinder.Domain.Enums;

namespace HostelFinder.Application.DTOs.Room.Requests;

public class RoomResponseDto
{
    public Guid Id { get; set; }
    public Guid HostelId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public RoomType RoomType { get; set; }
    public decimal? Size { get; set; }
    public decimal MonthlyRentCost { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateTime DateAvailable { get; set; }
    public List<string> ImageUrls { get; set; }
    public RoomDetailsResponseDto RoomDetailsDto { get; set; }
    public RoomAmenitiesResponseDto RoomAmenitiesDto { get; set; }
    public ICollection<ServiceCostResponseDto> ServiceCostsDto { get; set; }
}
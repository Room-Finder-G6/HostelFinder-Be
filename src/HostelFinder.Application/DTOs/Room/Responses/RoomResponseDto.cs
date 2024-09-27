using HostelFinder.Application.DTOs.RoomAmenities.Response;
using HostelFinder.Application.DTOs.RoomDetails.Response;
using HostelFinder.Application.DTOs.ServiceCost.Responses;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Enums;

namespace HostelFinder.Application.DTOs.Room.Requests;

public class RoomResponseDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal? Size { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateTime DateAvailable { get; set; }
    public RoomType RoomType { get; set; }
    public List<string> ImageUrls { get; set; }
    public RoomDetailsResponseDto RoomDetailsDto { get; set; }
    public RoomAmenitiesResponseDto RoomAmenitiesDto { get; set; }
    public List<ServiceCostResponseDto> ServiceCostsDto { get; set; }
}
﻿using HostelFinder.Application.DTOs.Amenity.Response;
using HostelFinder.Application.DTOs.RoomDetails.Response;
using HostelFinder.Application.DTOs.ServiceCost.Responses;
using HostelFinder.Domain.Enums;
using Microsoft.AspNetCore.Http;

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
    public IFormFile[] Images { get; set; }
    public RoomDetailsResponseDto RoomDetailsDto { get; set; }
    public List<AmenityResponse> AmenityResponses { get; set; }
    public ICollection<ServiceCostResponseDto> ServiceCostsDto { get; set; }
}
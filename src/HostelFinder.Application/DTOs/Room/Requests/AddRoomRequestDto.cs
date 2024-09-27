﻿using System.ComponentModel.DataAnnotations;
using HostelFinder.Application.DTOs.RoomAmenities.Request;
using HostelFinder.Application.DTOs.RoomDetails.Request;
using HostelFinder.Application.DTOs.ServiceCost.Request;
using HostelFinder.Domain.Enums;

namespace HostelFinder.Application.DTOs.Room.Requests;

public class AddRoomRequestDto
{
    [Required]
    public string Title { get; set; }
    [Required]
    [MaxLength(255)]
    public string Description { get; set; }
    public RoomType RoomType { get; set; }
    public List<string> ImageUrls { get; set; }
    public decimal? Size { get; set; }
    [Required]
    public decimal MonthlyRentCost { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateTime DateAvailable { get; set; }
    public Guid HostelId { get; set; }
    public Guid RoomTypeId { get; set; }
    public AddRoomAmenitiesDto RoomAmenities { get; set; }
    public AddRoomDetailsDto RoomDetails { get; set; }
    public List<AddServiceCostDto> ServiceCosts { get; set; }
}
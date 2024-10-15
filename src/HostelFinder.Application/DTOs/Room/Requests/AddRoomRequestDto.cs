﻿using System.ComponentModel.DataAnnotations;
using HostelFinder.Application.DTOs.Amenity.Request;
using HostelFinder.Application.DTOs.RoomDetails.Request;
using HostelFinder.Application.DTOs.ServiceCost.Request;
using HostelFinder.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace HostelFinder.Application.DTOs.Room.Requests;

public class AddRoomRequestDto
{
    [Required]
    public Guid HostelId { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    [MaxLength(255)]
    public string Description { get; set; }
    public IFormFile PrimaryImage { get; set; }
    public RoomType RoomType { get; set; }
    public IFormFile[] Images{ get; set; }
    public decimal Size { get; set; }
    [Required]
    public decimal MonthlyRentCost { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateTime DateAvailable { get; set; }
    public List<AddRoomAmenityDto> AddRoomAmenity { get; set; }
    public AddRoomDetailsDto RoomDetails { get; set; }
    public ICollection<AddServiceCostDto> ServiceCosts { get; set; } = new List<AddServiceCostDto>();
}
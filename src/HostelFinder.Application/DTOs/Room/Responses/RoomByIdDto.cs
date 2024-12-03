﻿using DocumentFormat.OpenXml.Office2010.ExcelAc;
using HostelFinder.Domain.Enums;

namespace HostelFinder.Application.DTOs.Room.Responses;

public class RoomByIdDto
{
    public Guid Id { get; set; }
    public string? HostelName { get; set; }
    public string? RoomName { get; set; }
    public int? Floor { get; set; }
    public int MaxRenters { get; set; }
    public float Size { get; set; }
    public bool IsAvailable { get; set; }
        
    public decimal Deposit {  get; set; }
    public decimal MonthlyRentCost { get; set; }
    public RoomType RoomType { get; set; }
    public DateTimeOffset CreatedOn { get; set; }

    public List<string>? ImageRoom { get; set; }
}
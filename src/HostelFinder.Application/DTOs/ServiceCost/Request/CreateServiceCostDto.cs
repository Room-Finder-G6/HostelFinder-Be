﻿using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Application.DTOs.ServiceCost.Request;

public class CreateServiceCostDto
{
    [Required]
    public Guid HostelId { get; set; }


    [Required]
    public Guid ServiceId { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Unit Cost phải là số dương")]
    public decimal UnitCost { get; set; }

    [Required]
    public DateTime EffectiveFrom { get; set; }
}
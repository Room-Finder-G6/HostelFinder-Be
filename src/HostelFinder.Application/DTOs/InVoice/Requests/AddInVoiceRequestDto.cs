﻿namespace HostelFinder.Application.DTOs.InVoice.Requests
{
    public class AddInVoiceRequestDto
    {
        public decimal TotalAmount { get; set; }
        public bool Status { get; set; }
        public DateTime DueDate { get; set; }
    }
}

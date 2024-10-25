using HostelFinder.Application.DTOs.InVoice.Requests;
using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Application.DTOs.ServiceCost.Request;

public class AddServiceCostDto
{
    [Required] 
    public string ServiceName { get; set; }
    public decimal Cost { get; set; }
    public decimal unitCost { get; set; }
    public int PreviousReading { get; set; }
    public int CurrentReading { get; set; }
    //public AddInVoiceRequestDto AddInVoiceRequestDto { get; set; }
}
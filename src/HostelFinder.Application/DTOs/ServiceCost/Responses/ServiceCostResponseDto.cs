namespace HostelFinder.Application.DTOs.ServiceCost.Responses
{
    public class ServiceCostResponseDto
    {
        public Guid HostelId { get; set; }  

        public Guid ServiceId { get; set; }

        public string? ServiceName { get; set; } 

        public string? HostelName { get; set; }

        public decimal UnitCost { get; set; }

        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public DateTimeOffset CreatedOn { get; set; } 
    }
}

namespace HostelFinder.Application.DTOs.MeterReading.Request
{
    public class CreateMeterReadingDto
    {
        public Guid roomId { get; set; }

        public Guid serviceId { get; set; }

        public int reading {  get; set; }

        public int billingMonth { get; set; }

        public int billingYear { get; set;}
    }
}

namespace HostelFinder.Application.DTOs.MeterReading.Request
{
    public class EditMeterReadingDto
    {
        public Guid RoomId { get; set; }
        public Guid ServiceId { get; set; }
        public int Reading { get; set; }
        public int BillingMonth { get; set; }
        public int BillingYear { get; set; }
    }
}

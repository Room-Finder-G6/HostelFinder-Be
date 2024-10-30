namespace HostelFinder.Domain.Entities
{
    public class HostelService
    {
        public Guid Id { get; set; }

        public Guid ServiceId { get; set; }
        
        public Guid HostelId { get; set; }

        public  Hostel Hostel { get; set; }

        public Service Services { get; set; }
    }
}

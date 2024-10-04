namespace HostelFinder.Application.DTOs.Service.Request
{
    public class ServiceCreateRequestDTO
    {
        public string ServiceName { get; set; }
        public Guid HostelId { get; set; }
        public int Price { get; set; }
    }
}

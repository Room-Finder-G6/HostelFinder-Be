namespace HostelFinder.Application.DTOs.Service.Response
{
    public class ServiceResponseDTO
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public Guid HostelId { get; set; }
        public int Price { get; set; }
    }
}

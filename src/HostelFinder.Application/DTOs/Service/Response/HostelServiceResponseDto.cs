namespace HostelFinder.Application.DTOs.Service.Response
{
    public class HostelServiceResponseDto
    {
        public Guid ServiceId { get; set; }

        public Guid HostelId { get; set; }

        public string? ServiceName { get; set; }

        public string? HostelName {  get; set; }
    }
}

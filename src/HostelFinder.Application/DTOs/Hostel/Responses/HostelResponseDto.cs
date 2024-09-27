namespace HostelFinder.Application.DTOs.Hostel.Responses
{
    public class HostelResponseDto
    {
        public Guid Id { get; set; }
        public string HostelName { get; set; }
        public string? Description { get; set; }
        public string Address { get; set; }
        public int NumberOfRooms { get; set; }
        public float Rating { get; set; }
    }
}

namespace HostelFinder.Application.DTOs.Room.Requests;

public class UpdatePostRequestDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Status { get; set; }
    public DateTime DateAvailable { get; set; }
}
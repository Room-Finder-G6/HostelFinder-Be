using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Application.DTOs.Room.Requests;

public class UpdatePostRequestDto
{
    public Guid HostelId { get; set; }
    public Guid RoomId { get; set; }
    [MaxLength(50)]
    public string Title { get; set; }
    [MaxLength(100)]
    public string Description { get; set; }
    public bool Status { get; set; }
    public DateTime DateAvailable { get; set; }
}
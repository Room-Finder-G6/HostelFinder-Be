using HostelFinder.Application.DTOs.Image.Responses;

namespace HostelFinder.Application.DTOs.Room.Requests;

public class ListPostResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public decimal Size { get; set; }
    public string Address { get; set; }
    public List<ImageResponseDto> Image { get; set; }
    public decimal MonthlyRentCost { get; set; }
}
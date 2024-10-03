namespace HostelFinder.Application.DTOs.Room.Requests;

public class ListRoomResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Address { get; set; }
    public decimal Size { get; set; }
    public string PrimaryImageUrl { get; set; }

    public decimal MonthlyRentCost { get; set; }
}
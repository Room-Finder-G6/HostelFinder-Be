namespace HostelFinder.Application.DTOs.Post.Responses;

public class ListPostsResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Address { get; set; }
    public decimal MonthlyRentCost { get; set; }
    public decimal Size { get; set; }
    public string FirstImage { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
}
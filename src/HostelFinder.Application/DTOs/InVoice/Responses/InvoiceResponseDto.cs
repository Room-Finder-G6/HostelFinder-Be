namespace HostelFinder.Application.DTOs.InVoice.Responses
{
    public class InvoiceResponseDto
    {
        public Guid Id { get; set; }
        public decimal TotalAmount { get; set; }
        public bool Status { get; set; }
        public DateTime DueDate { get; set; }
    }
}

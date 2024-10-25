using RoomFinder.Domain.Common;

namespace HostelFinder.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public Guid ServiceCostId { get; set; }
        public decimal TotalAmount { get; set; }
        public bool Status { get; set; }
        public DateOnly DueDate {  get; set; }
        public virtual ICollection<ServiceCost> ServiceCost { get; set; }
    }
}
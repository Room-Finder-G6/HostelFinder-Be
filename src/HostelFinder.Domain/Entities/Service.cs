using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RoomFinder.Domain.Common;

namespace HostelFinder.Domain.Entities
{
    public class Service : BaseEntity
    {
        public string ServiceName { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public bool IsBillable { get; set; }
        public bool IsUsageBased { get; set; }
        public ICollection<HostelServices> HostelServices { get; set; } = new List<HostelServices>();
        public virtual ICollection<ServiceCost> ServiceCosts { get; set; } = new List<ServiceCost>();
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
    }
}
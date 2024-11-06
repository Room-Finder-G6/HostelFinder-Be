using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RoomFinder.Domain.Common;

namespace HostelFinder.Domain.Entities
{
    public class InvoiceDetail : BaseEntity
    {
        [ForeignKey("Invoice")]
        [Required]
        public Guid InvoiceId { get; set; }

        [ForeignKey("Service")]
        [Required]
        public Guid ServiceId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitCost { get; set; } 

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ActualCost { get; set; }

        public int? NumberOfCustomer { get; set; }

        public int PreviousReading { get; set; }
        public int CurrentReading { get; set; }

        [Required]
        public DateTime BillingDate { get; set; } 

        // Navigation
        public virtual Invoice Invoice { get; set; }
        public virtual Service Service { get; set; }
    }
}

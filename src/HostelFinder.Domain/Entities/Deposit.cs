using RoomFinder.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Domain.Entities
{
    public class Deposit : BaseEntity
    {
        [Required]
        [Key]
        public Guid UserId { get; set; }

        [Required]
        public Guid RoomId { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        public virtual User User { get; set; }
        public virtual Room Room { get; set; }
    }
}
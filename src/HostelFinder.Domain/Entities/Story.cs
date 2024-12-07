using HostelFinder.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Domain.Entities
{
    public class Story : BaseEntity
    {
        [Required]
        [MaxLength(512)]
        public string Title { get; set; }
        [Required]
        [MaxLength(3000)]
        public string Description { get; set; }
        public bool Status { get; set; } = true;
        public DateTime DateAvailable { get; set; }
        public Guid MembershipServiceId { get; set; }
    }
}

using RoomFinder.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HostelFinder.Domain.Entities
{
    public class Review : BaseEntity
    {
        public string Comment {  get; set; }
        public int rating { get; set; }
        public DateTime ReviewDate { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        [ForeignKey("Hostel")]
        public Guid HostelId { get; set; }
        public virtual User User { get; set; }
        public virtual Hostel Hostel { get; set; }
    }
}

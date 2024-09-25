using RoomFinder.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Domain.Entities
{
    public class RoomType : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string TypeName{  get; set; }
        public virtual ICollection<Room> Rooms { get; set; }    
    }
}
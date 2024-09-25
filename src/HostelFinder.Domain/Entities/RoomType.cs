using RoomFinder.Domain.Common;
using System.ComponentModel.DataAnnotations;
using HostelFinder.Domain.Enums;

namespace HostelFinder.Domain.Entities
{
    public class RoomType : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public RoomTypeEnum TypeName{  get; set; }
        public virtual ICollection<Room> Rooms { get; set; }    
    }
}
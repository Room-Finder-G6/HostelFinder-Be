using RoomFinder.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Domain.Entities
{
    public class RoomType : BaseEntity
    {
        [Required]
        public string Room_Type {  get; set; }
    }
}
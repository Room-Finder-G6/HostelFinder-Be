using RoomFinder.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Domain.Entities
{
    public class BlackListToken : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Token { get; set; } = string.Empty;
    }
}

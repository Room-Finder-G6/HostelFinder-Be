using RoomFinder.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Domain.Entities
{
    public class Tenant : BaseEntity 
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [EmailAddress]
        [Required]
        public required string Email { get; set; }

        [Required]
        [Phone]
        public required string Phone { get; set; }


        public DateTime DateOfBirth { get; set; }

        public string? Province { get; set; }

        public string? District { get; set; }

        public string? Commune { get; set; }

        [MaxLength(255)]
        public string? DetailAddress { get; set; }

    }
}

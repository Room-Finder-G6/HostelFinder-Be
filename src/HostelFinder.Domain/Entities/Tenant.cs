using HostelFinder.Domain.Enums;
using RoomFinder.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Domain.Entities
{
    public class Tenant : BaseEntity 
    {
        [Required]
        [MaxLength(100)]
        public required string FullName { get; set; }

        public string? AvatarUrl { get; set; }

        [EmailAddress]
        [Required]
        public required string Email { get; set; }

        [Required]
        [Phone]
        public required string Phone { get; set; }

        public string? Description { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Province { get; set; }

        public string? District { get; set; }

        public string? Commune { get; set; }

        [MaxLength(255)]
        public string? DetailAddress { get; set; }

        /// <summary>
        /// CCCD
        /// </summary>
        [Required]
        [MaxLength(12)]
        public string IdentityCardNumber { get; set; }

        [MaxLength(255)]
        public string FrontImageUrl { get; set; }

        [MaxLength(255)]
        public string BackImageUrl { get; set; } 

        public TemporaryResidenceStatus TemporaryResidenceStatus { get; set; }


        // Navigation properties
        public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

        public virtual ICollection<RoomTenancy> RoomTenancies { get; set; } = new List<RoomTenancy>();



    }
}

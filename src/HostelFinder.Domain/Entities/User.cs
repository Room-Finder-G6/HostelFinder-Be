using RoomFinder.Domain.Common;
using System.ComponentModel.DataAnnotations;
using HostelFinder.Domain.Enums;

namespace HostelFinder.Domain.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MaxLength(256)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; }

        [Phone]
        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(255)]
        public string? AvatarUrl { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        [MaxLength(255)]
        public bool? IsEmailConfirmed { get; set; }

        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpires { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
        public virtual ICollection<Hostel>? Hostels { get; set; }
        public virtual ICollection<BookingRequest>? BookingRequests { get; set; }
        public virtual Wishlist? Wishlists { get; set; }
    }
}
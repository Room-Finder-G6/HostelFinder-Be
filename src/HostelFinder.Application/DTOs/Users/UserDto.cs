using HostelFinder.Domain.Enums;
using RoomFinder.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace HostelFinder.Application.DTOs.Users
{
    public class UserDto : BaseEntity
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
        public bool IsActive { get; set; }
    }
}

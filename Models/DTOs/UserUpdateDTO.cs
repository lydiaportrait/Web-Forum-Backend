using System.ComponentModel.DataAnnotations;

namespace portrait_forum.Models
{
    public class UserUpdateDTO
    {
        [Required]
        [StringLength(32, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(32, MinimumLength = 2)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [StringLength(2000, MinimumLength = 2)]
        public string Description { get; set; } = string.Empty;
        [StringLength(64, MinimumLength = 2)]
        public string OldPassword { get; set; } = string.Empty;
        [StringLength(64, MinimumLength = 2)]
        public string NewPassword { get; set; } = string.Empty;
    }
}

using System.ComponentModel.DataAnnotations;

namespace portrait_forum.Models
{
    public class UserCreationDTO
    {
        [Required]
        [StringLength(32, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(32, MinimumLength = 2)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        [StringLength(64, MinimumLength = 2)]
        public string Password { get; set; } = string.Empty;
    }
}

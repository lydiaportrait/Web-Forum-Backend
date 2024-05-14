using System.ComponentModel.DataAnnotations;

namespace portrait_forum.Models
{
    public class ConversationPostDTO
    {
        [Required]
        [StringLength(32, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public long BoardId { get; set; }
    }
}

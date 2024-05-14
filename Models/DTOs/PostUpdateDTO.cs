using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace portrait_forum.Models
{
    public class PostUpdateDTO
    {
        [Required]
        [StringLength(400, MinimumLength = 1)]
        public string Content { get; set; } = string.Empty;
    }
}

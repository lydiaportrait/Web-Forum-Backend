using System.ComponentModel.DataAnnotations;

namespace portrait_forum.Models
{
    public class ConversationDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long BoardId { get; set; }
    }
}

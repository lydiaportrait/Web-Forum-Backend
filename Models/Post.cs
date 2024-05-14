using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace portrait_forum.Models
{
    public class Post
    {
        public long Id { get; set; }
        public User Owner { get; set; } = new User();
        public long OwnerID { get; set; }
        public long ConversationID { get; set; }
        public Conversation Conversation { get; set; } = null!;
        public string Content { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
    }
}

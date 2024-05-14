using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace portrait_forum.Models
{
    public class PostReturnDTO
    {
        public long Id { get; set; }
        public UserDTO Owner { get; set; }
        public long ConversationID { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }

        public PostReturnDTO(Post post)
        {
            Id = post.Id;
            ConversationID = post.ConversationID;
            Content = post.Content;
            DateCreated = post.DateCreated;
            Owner = new UserDTO(post.Owner);
        }
        
        public PostReturnDTO() { }
    }
}

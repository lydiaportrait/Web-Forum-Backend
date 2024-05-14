using System.ComponentModel.DataAnnotations;

namespace portrait_forum.Models
{
    public class Conversation
    {
        public long Id { get; set; }
        public User Owner { get; set; } = new User();
        public long OwnerID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long BoardId { get; set; }
        public Board Board { get; set; } = null!;
        public ICollection<Post> Posts { get;} = new List<Post>();
        public DateTime DateCreated { get; set; }
    }
}

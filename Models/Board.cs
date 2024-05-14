using System.ComponentModel.DataAnnotations;

namespace portrait_forum.Models
{
    public class Board
    {
        public long Id { get; set; }
        public User Owner { get; set; } = new User();
        public long OwnerID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description {  get; set; } = string.Empty;
        public ICollection<Conversation> Conversations { get;} = new List<Conversation>();
        public DateTime DateCreated { get; set; }
    }
}

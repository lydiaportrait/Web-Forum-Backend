using System.ComponentModel.DataAnnotations;

namespace portrait_forum.Models
{
    public class ConversationReturnDTO
    {
        public long Id { get; set; }
        public UserDTO Owner { get; set; } = new UserDTO();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long BoardId { get; set; }
        public DateTime DateCreated { get; set; }

        public ConversationReturnDTO(Conversation conversation)
        {
            Id = conversation.Id;
            Name = conversation.Name;
            Description = conversation.Description;
            BoardId = conversation.BoardId;
            DateCreated = conversation.DateCreated;
            Owner = new UserDTO(conversation.Owner);
        }
        
        public ConversationReturnDTO() { }
    }
}

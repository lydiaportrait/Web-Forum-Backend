using System.ComponentModel.DataAnnotations;

namespace portrait_forum.Models
{
    public class BoardDTO
    {
        public long Id { get; set; }
        public UserDTO Owner { get; set; } = new UserDTO();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }

        public BoardDTO(Board board)
        {
            Id = board.Id;
            Owner = new UserDTO(board.Owner);
            Name = board.Name;
            Description = board.Description;
        }
        
        public BoardDTO() { }
    }
}

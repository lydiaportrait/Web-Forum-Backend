using System.ComponentModel.DataAnnotations;

namespace portrait_forum.Models
{
    public class UserLoginDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Password {  get; set; } = string.Empty;
    }
}

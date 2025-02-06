using System.ComponentModel.DataAnnotations;

namespace Entregable_Universities.Models
{
    public class UserLoginsModel
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}

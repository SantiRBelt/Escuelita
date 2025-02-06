using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entregable_Universities.Models
{
    public class CategoryModel: BaseEntityModel
    {
        //[Key]
        //public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; set; } = string.Empty;
        //[Required]
        //public string idCourse { get; set; } = string.Empty;
        //public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

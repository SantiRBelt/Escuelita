using System.ComponentModel.DataAnnotations;

namespace Entregable_Universities.Models
{
    public enum Level
    {
        Basic,
        Medium,
        Advanced,
        Expert
    }
    public class CourseModel : BaseEntityModel
    {
        //[Key]
        //public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public Level Level { get; set; } = Level.Basic;
        [Required]
        public List<int> idCategories { get; set; } = new List<int>();
        //public Chapter Chapter { get; set; } = new Chapter();
       // public string idStudent { get; set; } = string.Empty;

        //public DateTime CreatedAt { get; set; } = DateTime.Now;


    }
}

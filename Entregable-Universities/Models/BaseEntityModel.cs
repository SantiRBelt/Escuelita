using System.ComponentModel.DataAnnotations;

namespace Entregable_Universities.Models
{
    public class BaseEntityModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));
    }
}

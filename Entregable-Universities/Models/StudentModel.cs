using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entregable_Universities.Models
{
    public class StudentModel:BaseEntityModel
    {
        [Required]
        public string DocType { get; set; } = string.Empty;
        [Required]
        public int Document { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Course { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string District { get; set; } = string.Empty;
        [Required]
        public string Attendant { get; set; } = string.Empty;
        [Required]
        public bool Active { get; set; }

    }
}

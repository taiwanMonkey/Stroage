using System.ComponentModel.DataAnnotations;

namespace Stroage.API.Models
{
    public class Person
    {
        [MaxLength(20)]
        [Required]
        public string Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(20)]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

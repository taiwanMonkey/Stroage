using System.ComponentModel.DataAnnotations;

namespace Stroage.API.Models
{
    public class Storehouse
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}

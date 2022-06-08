using System.ComponentModel.DataAnnotations;

namespace Stroage.API.Models
{
    public class Bin
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public int StorehouseId { get; set; }
        public virtual Storehouse Storehouse { get; set; }  
    }
}

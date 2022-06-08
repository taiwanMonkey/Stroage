
using System.ComponentModel.DataAnnotations;

namespace Stroage.API.Models
{
    public class Pack
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int MaterialId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public virtual Material Material { get; set; }
    }
}

using Stroage.API.Enum;
using System.ComponentModel.DataAnnotations;

namespace Stroage.API.Models
{
    public class Material
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }
        public virtual MaterialType Type { get; set; }
    }
}

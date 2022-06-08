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

        [DataType(DataType.DateTime)]
        public DateTime? InTime { get; set; }
        public int? PackId { get; set; }
        public virtual Pack? Pack { get; set; }
        public virtual Storehouse? Storehouse { get; set; }  
    }
}

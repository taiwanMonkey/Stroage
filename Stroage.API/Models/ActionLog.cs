using System.ComponentModel.DataAnnotations;

namespace Stroage.API.Models
{
    public class ActionLog
    {
        [Required]
        public int Id { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public bool IsIn { get; set; }
        public string UserId { get; set; }
        public int PackId { get; set; }
        public int BinId { get; set; }
        public virtual Person Person { get; set; }
        public virtual Pack Pack { get; set; }
        public virtual Bin Bin { get; set; }
    }
}

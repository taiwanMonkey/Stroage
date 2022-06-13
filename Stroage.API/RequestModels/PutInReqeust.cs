using System.ComponentModel.DataAnnotations;

namespace Stroage.API.RequestModels
{
    public class PutInReqeust
    {
        [Required]
        public string StorageToken { get; set; }
        [Required]
        public string MaterialDescirption { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string BinName { get; set; }
    }
}

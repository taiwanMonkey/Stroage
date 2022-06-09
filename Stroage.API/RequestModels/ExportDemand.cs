using System.ComponentModel.DataAnnotations;

namespace Stroage.API.RequestModels
{
    public class ExportDemand
    {
        [Required]
        public string MateriralDesc { get; set; }
        [Required]
        public int Demand { get; set; }
    }
}

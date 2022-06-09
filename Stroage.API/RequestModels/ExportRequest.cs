namespace Stroage.API.RequestModels
{
    public class ExportRequest
    {
        public Person? User { get; set; }
        public IEnumerable<ExportDemand>? Demands { get; set; }
    }
}

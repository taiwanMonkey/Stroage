namespace Stroage.API.RequestModels
{
    public class BinDetail
    {
        public int Id { get; set; }
        public string BinName { get; set; }
        public int? Quantity { get; set; }
        public string? MaterialDesc { get; set; }
    }
}

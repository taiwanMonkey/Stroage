namespace Stroage.API.RequestModels
{
    public class DeserializedActionLog
    {
        public string UserName { get; set; }
        public string BinName { get; set; }
        public string MaterialDesc { get; set; }
        public int Quantity { get; set; }
        public string Operation { get; set; }
        public DateTime OpTime { get; set; }
    }
}

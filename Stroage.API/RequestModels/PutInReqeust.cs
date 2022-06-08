namespace Stroage.API.RequestModels
{
    public class PutInReqeust
    {
        public string PersonId { get; set; }
        public string Password { get; set; }
        public string MaterialDescirption { get; set; }
        public int Quantity { get; set; }
        public string BinName { get; set; }
    }
}

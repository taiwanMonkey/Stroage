namespace Stroage.API.RequestModels
{
    public class StorehouseDetail
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public IEnumerable<BinDetail> Bins { get; set; }

    }
}

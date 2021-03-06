using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stroage.API.RequestModels;

namespace Stroage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorehouseController : ControllerBase
    {
        private readonly StorageContext _context;
        public StorehouseController(StorageContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<StorehouseDetail> Get()
        {
            var houses = _context.Storehouses
                .Include(x => x.Bins)
                .ThenInclude(b => b.Pack)
                .ThenInclude(p => p.Material)
                .Select(h => new StorehouseDetail
                {
                    Name = h.Name,
                    Id = h.Id,
                    Bins = h.Bins.Select(b => new BinDetail
                                { 
                                  BinName = b.Name, 
                                  Id = b.Id,
                                  MaterialDesc = b.Pack.Material.Description,
                                  Quantity = b.Pack.Quantity
                                })
                })
                .ToList();
            return houses;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, int capacity)
        {
            bool inValid = string.IsNullOrEmpty(name) || name.Length > 50 || capacity < 0;
            if(inValid)
                return BadRequest(ModelState);


            Storehouse house = _context.Storehouses.FirstOrDefault(sh => sh.Name == name);
            if (house != null)
                return BadRequest("重複的倉庫名。");
            if (capacity > 1024)
                return BadRequest("庫位量不可超過1024。");
            house = new Storehouse()
            {
                Name = name,
                Bins = new List<Bin>()
            };
            for (int i = 1; i <= capacity; i++)
            {
                house.Bins.Add(new Bin()
                {
                    Name = $"{name}.{i:D4}",
                });
            }
            _context.Storehouses.Add(house);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}

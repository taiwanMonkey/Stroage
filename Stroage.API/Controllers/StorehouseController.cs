using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IEnumerable<Storehouse> Get()
        {
            return _context.Storehouses.ToArray();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, int capacity)
        {
            Storehouse house = _context.Storehouses.FirstOrDefault(sh => sh.Name == name);
            if (house != null)
                return BadRequest("重複的倉庫名。");

            house = new Storehouse()
            {
                Name = name,
                Bins = new List<Bin>()
            };
            for (int i = 1; i <= capacity; i++)
            {
                house.Bins.Add(new Bin()
                {
                    Name = $"{name}.{i}",
                });
            }
            _context.Storehouses.Add(house);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}

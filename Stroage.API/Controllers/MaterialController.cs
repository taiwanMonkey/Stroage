using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stroage.API.Enum;

namespace Stroage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly StorageContext _context;

        public MaterialController(StorageContext storageContext)
        {
            _context = storageContext;
        }

        [HttpPost]
        public IActionResult Create(string description, string type)
        {
            var m = _context.Materials.FirstOrDefault(x => x.Description == description);
            if (m is not null)
                return BadRequest("此物料已定義");
            
            Material material = new()
            {
                Description = description,
                Type = type == "SMT"? MaterialType.SMT : MaterialType.DIP
            };

            _context.Materials.Add(material);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IEnumerable<Material> Get()
        {
            return _context.Materials.ToArray();
        }
    }
}

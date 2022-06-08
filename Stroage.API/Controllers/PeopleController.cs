using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stroage.API.Models;

namespace Stroage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly StorageContext _context;

        public PeopleController(StorageContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Person>> Create(Person person)
        {
            Person p = await _context.People.FirstOrDefaultAsync(p => p.Id == person.Id);
            if (p != null)
                return BadRequest("此ID已被註冊");
            _context.People.Add(person);
            _context.SaveChanges();
            return Ok(person);
        }

        [HttpGet]
        public IEnumerable<Person> Get()
        {
            var pps = _context.People.ToArray();
            return pps;
        }
    }
}

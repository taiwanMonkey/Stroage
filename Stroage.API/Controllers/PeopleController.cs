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
            bool inValid = string.IsNullOrEmpty(person.Id) || person.Id.Length > 20 ||
                string.IsNullOrEmpty(person.Name) || person.Name.Length > 50 ||
                string.IsNullOrEmpty(person.Password) || person.Password.Length > 50;

            if (inValid)
                return BadRequest(ModelState);

            Person? p = await _context.People.FirstOrDefaultAsync(p => p.Id == person.Id);
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

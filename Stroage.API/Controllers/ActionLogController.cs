using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stroage.API.RequestModels;

namespace Stroage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionLogController : ControllerBase
    {
        private readonly StorageContext _context;

        public ActionLogController(StorageContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<DeserializedActionLog>> Get(DateTime? beginTime = null, DateTime? endTime = null)
        {
            if (!beginTime.HasValue)
                beginTime = DateTime.Now.AddDays(-7);
            if (!endTime.HasValue)
                endTime = DateTime.Now;            
            var logs = await _context.ActionLogs
                .Include(l => l.Person)
                .Include(l => l.Pack)
                .ThenInclude(p => p.Material)
                .Include(l => l.Bin)
                .AsNoTracking()
                .Where(l => l.CreateTime >= beginTime && l.CreateTime <= endTime)
                .Select(l => new DeserializedActionLog
                {
                    UserName = l.Person.Name,
                    BinName = l.Bin.Name,
                    MaterialDesc = l.Pack.Material.Description,
                    Quantity = l.Pack.Quantity,
                    Operation = l.IsIn ? "入庫" : "出庫",
                    OpTime = l.CreateTime,
                })
                .ToArrayAsync();
            return logs;
        }
    }
}

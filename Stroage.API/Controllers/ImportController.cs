using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stroage.API.RequestModels;

namespace Stroage.API.Controllers
{
    /// <summary>
    /// 入庫
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly StorageContext _context;
        private readonly ILogger<ImportController> _logger;

        public ImportController(StorageContext context, ILogger<ImportController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int? id = null;
            Storehouse[] shList = await GetEmptyBinsOfHouses();
            Material[] materials = await GetMaterialsAsync();
            ImportPreparation preparation = new ImportPreparation()
            {
                Houses = shList,
                Materials = materials
            };
            return Ok(preparation);
        }

        private async Task<Storehouse[]> GetEmptyBinsOfHouses()
        {
            var shList = await _context.Storehouses
                .AsNoTracking()
                .Include(sh => sh.Bins)
                .ThenInclude(b => b.Pack)
                .ToArrayAsync();
            foreach (Storehouse storehouse in shList)
            {
                storehouse.Bins = storehouse.Bins.Where(b => b.Pack is null).ToList();
            }

            return shList;
        }

        private async Task<Material[]> GetMaterialsAsync()
        {
            return await _context.Materials.ToArrayAsync();
        }

        [HttpPost]
        public async Task<IActionResult> PutIn(PutInReqeust model)
        {
            _logger.LogInformation($"{nameof(ImportController)}.{nameof(PutIn)}{{ {model.BinName} }}");
            bool inValidInput = model is null || string.IsNullOrEmpty(model.StorageToken) ||  string.IsNullOrEmpty(model.MaterialDescirption) || 
                string.IsNullOrEmpty(model.BinName) || model.Quantity < 1;

            if (inValidInput)
                return BadRequest(ModelState);

            try
            {
                Person? user = await _context.People.FirstOrDefaultAsync(p => p.Token.ToString() == model.StorageToken);
                if (user is null)
                    return Unauthorized("尚未登入");

                Material? material = await _context.Materials.FirstOrDefaultAsync(m => m.Description == model.MaterialDescirption);
                if (material is null)
                    return BadRequest("此物料未定義，請先定義物料。");

                Bin? bin = await _context.Bins.FirstOrDefaultAsync(b => b.Name == model.BinName);
                if (bin is null)
                    return BadRequest("查無此庫位碼");
                if (bin.PackId > 0)
                    return BadRequest("此庫位已有物料，請使用其他庫位");

                Pack pack = new Pack()
                {
                    Material = material,
                    MaterialId = material.Id,
                    Quantity = model.Quantity,
                };
                _context.Pack.Add(pack);
                bin.Pack = pack;
                bin.InTime = DateTime.Now;

                ActionLog actionLog = new ActionLog()
                {
                    Person = user,
                    Bin = bin,
                    Pack = pack,
                    CreateTime = DateTime.Now,
                    IsIn = true,
                    Quantity = model.Quantity
                };
                _context.ActionLogs.Add(actionLog);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    UserId = user.Id,
                    BinName = bin.Name,
                    MaterialDesc = material.Description,
                    Quantity = pack.Quantity,
                    Time = actionLog.CreateTime
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}: {ex.StackTrace}");
                return NotFound(ex.Message);
                throw;
            }
        }
    }
}

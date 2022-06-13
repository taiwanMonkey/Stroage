using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stroage.API.RequestModels;

namespace Stroage.API.Controllers
{
    /// <summary>
    /// 出庫
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private StorageContext _context;
        public ExportController(StorageContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Export(ExportRequest model)
        {
            bool inValid = model is null || model.User is null || model.Demands is null ||
                string.IsNullOrEmpty(model.User.Id) || string.IsNullOrEmpty(model.User.Password) ||
                !model.Demands.Any() || model.Demands.Any(d => string.IsNullOrEmpty(d.MateriralDesc));

            if(inValid)
                return BadRequest(ModelState);

            Person? person = await _context.People.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == model.User.Id && p.Password == model.User.Password);

            if(person is null)
                return Unauthorized("帳號或密碼錯誤");

            List<Bin> selectedBins = new ();
            List<ExportDemand> lackMaterials = new();
            List<ExportingBin> exportings = new();
            foreach (ExportDemand demand in model.Demands)
            {
                var availbles = await GetPacksInHouseAsync(demand.MateriralDesc);
                while(demand.Demand > 0)
                {
                    if (availbles is null || availbles.Count == 0)
                    {
                        lackMaterials.Add(demand);
                        break;
                    }
                    TakeOnePack(model, selectedBins, demand, availbles, exportings);
                }
            }
            await _context.SaveChangesAsync();

            return Ok(new { exportings, lackMaterials });
        }

        [HttpPost]
        [Route("Single")]
        public async Task<IActionResult> ExportSingle(ExportSingleParam param)
        {
            bool inValid = param is null || string.IsNullOrEmpty(param.BinName) || string.IsNullOrEmpty(param.StorageToken);
            if (inValid)
                return NotFound();

            var person = _context.People.FirstOrDefault(p => p.Token.ToString() == param.StorageToken);
            if(person is null)
                return Unauthorized();

            var bin = _context.Bins.Include(b => b.Pack).FirstOrDefault(b => b.Name == param.BinName);
            if (bin is null || bin.Pack is null)
                return NotFound();

            LogExport(bin, person);
            bin.InTime = null;
            bin.PackId = null;
            await _context.SaveChangesAsync();
            return Ok();
        }

        private void TakeOnePack(ExportRequest model, List<Bin> selectedBins, ExportDemand demand, List<Bin> availbles, List<ExportingBin> exportings)
        {
            Bin? chosenBin = availbles.First();
            selectedBins.Add(chosenBin);
            demand.Demand -= chosenBin.Pack.Quantity;
            availbles.Remove(chosenBin);
            exportings.Add(new ExportingBin
            {
                BinName = chosenBin.Name,
                MaterialDesc = chosenBin.Pack.Material.Description,
                Quantity = chosenBin.Pack.Quantity
            });
            LogExport(chosenBin, model.User);
            chosenBin.Pack = null;
            return;
        }

        private void LogExport(Bin chosenBin, Person? user)
        {
            ActionLog log = new();

            log.BinId = chosenBin.Id;
            log.PackId = chosenBin.Pack.Id;
            log.PersonId = user.Id;
            log.CreateTime = DateTime.Now;
            log.Quantity = chosenBin.Pack.Quantity;
            log.IsIn = false;

            _context.ActionLogs.Add(log);
            
        }

        private async Task<List<Bin>>? GetPacksInHouseAsync(string description)
        {
            var bins = await _context.Bins
                .Include(b => b.Pack)
                .ThenInclude(p => p.Material)
                .Where(b => b.Pack.Material.Description == description)
                .OrderByDescending(b => b.InTime)
                .ToListAsync();
            return bins;
        }
    }
}

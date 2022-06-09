﻿using Microsoft.AspNetCore.Http;
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

        public ImportController(StorageContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PutIn(PutInReqeust model)
        {
            bool inValidInput = model is null || string.IsNullOrEmpty(model.PersonId) || 
                string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.MaterialDescirption) || 
                string.IsNullOrEmpty(model.BinName) || model.Quantity < 1;

            if (inValidInput)
                return BadRequest(ModelState);

            Person? user = await _context.People.FirstOrDefaultAsync(p => p.Id == model.PersonId && p.Password == model.Password);
            if (user is null)
                return Unauthorized("帳號或密碼錯誤");

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
    }
}

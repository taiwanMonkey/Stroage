using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stroage.API.Models;
using Stroage.API.RequestModels;
using System.Collections.Generic;
using System.Text;

namespace Storage.MVC.Controllers
{
    public class ImportBinController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ImportBinController> _logger;

        public ImportBinController(IHttpClientFactory httpClientFactory, ILogger<ImportBinController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string binName = "", string hint = "")
        {
            var token = Request.Cookies["StorageToken"];
            if(string.IsNullOrEmpty(token))
                return Unauthorized();

            HttpClient client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7114/api/Import");
            if(!response.IsSuccessStatusCode)
                return NotFound();
            var content = await response.Content.ReadAsStringAsync();
            var housesAndMaterials = JsonConvert.DeserializeObject<ImportPreparation>(content);

            ViewData["Houses"] = housesAndMaterials.Houses;
            ViewData["Materials"] = housesAndMaterials.Materials;
            ViewData["IDtoken"] = token;
            ViewData["S_UserName"] = Request.Cookies["S_UserName"];
            ViewData["Hint"] = hint;
            
            return View("Index", binName);
        }

        [HttpPost]
        public async Task<IActionResult> Import(PutInReqeust request)
        {
            bool inValid = request.Quantity < 0 || request.Quantity > 40_000 ||
                string.IsNullOrEmpty(request.MaterialDescirption) ||
                string.IsNullOrEmpty(request.BinName) ||
                string.IsNullOrEmpty(request.StorageToken);
            if (inValid)
                return BadRequest();

            string j = JsonConvert.SerializeObject(request);
            HttpContent content = new StringContent(j, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            var result = await client.PostAsync(@"https://localhost:7114/api/Import", content);
            var str = JsonConvert.SerializeObject(await result.Content.ReadAsStringAsync());
            
            if (!result.IsSuccessStatusCode)
            {
                return Redirect("index");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}

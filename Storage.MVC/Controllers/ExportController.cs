using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stroage.API.RequestModels;
using System.Text;

namespace Storage.MVC.Controllers
{
    public class ExportController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ExportController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(string binName)
        {
            if(string.IsNullOrEmpty(binName))
                return NotFound();

            var token = Request.Cookies["StorageToken"].ToString();
            if(string.IsNullOrEmpty(token))
                return Unauthorized();

            var client = _httpClientFactory.CreateClient();
            ExportSingleParam param = new()
            {
                BinName = binName,
                StorageToken = token
            };
            var j = JsonConvert.SerializeObject(param);
            HttpContent content = new StringContent(j, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("https://localhost:7114/api/Export/Single", content );
            if (!result.IsSuccessStatusCode)
                return BadRequest("出庫失敗");

            return Redirect("../Home/Index");
        }
    }
}

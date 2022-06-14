using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Storage.MVC.Controllers
{
    public class StorehouseDeclareController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public StorehouseDeclareController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            if (!Request.Cookies.ContainsKey("StorageToken"))
                return Unauthorized();
            var token = Request.Cookies["StorageToken"].ToString();
            if (token == String.Empty)
                return Unauthorized();
            ViewData["IDtoken"] = token;
            ViewData["S_UserName"] = Request.Cookies["S_UserName"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Declare(string name, int capacity)
        {
            if (!Request.Cookies.ContainsKey("StorageToken"))
                return Unauthorized();
            var token = Request.Cookies["StorageToken"].ToString();
            if (token == String.Empty)
                return Unauthorized();
            if (string.IsNullOrEmpty(name) || capacity <= 0 || capacity > 500)
                return BadRequest();

            var client = _httpClientFactory.CreateClient();
            object param = new { name, capacity };
            string j = JsonConvert.SerializeObject(param);
            HttpContent content = new StringContent(j, Encoding.UTF8, "application/json");
            var result = await client.PostAsync($"https://localhost:7114/api/Storehouse?name={name}&capacity={capacity}", content);
            return RedirectToAction("Index", "Home");
        }
    }
}

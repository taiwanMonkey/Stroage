using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Storage.MVC.Controllers
{
    public class MaterialDeclareController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MaterialDeclareController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if(!Request.Cookies.ContainsKey("StorageToken"))
                return Unauthorized();
            var token = Request.Cookies["StorageToken"].ToString();
            if(token == String.Empty)
                return Unauthorized();
            ViewData["IDtoken"] = token;
            ViewData["S_UserName"] = Request.Cookies["S_UserName"];

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Declare(string description, string type)
        {
            if(string.IsNullOrEmpty(description) || string.IsNullOrEmpty(type))
                return BadRequest();

            if (!Request.Cookies.ContainsKey("StorageToken"))
                return Unauthorized();
            var token = Request.Cookies["StorageToken"].ToString();
            if (token == String.Empty)
                return Unauthorized();

            var client = _httpClientFactory.CreateClient();
            object param = new { description, type };
            string j = JsonConvert.SerializeObject(param);
            HttpContent content = new StringContent(j, Encoding.UTF8, "application/json");
            var result = await client.PostAsync($"https://localhost:7114/api/Material?description={description}&type={type}", content);
            return RedirectToAction("Index", "Home");
        }
    }
}

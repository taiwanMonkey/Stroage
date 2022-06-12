using Microsoft.AspNetCore.Mvc;
using Storage.MVC.Models;
using System.Diagnostics;
using System.Net;
using Stroage.API.Models;
using Newtonsoft.Json;
using Stroage.API.RequestModels;

namespace Storage.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpCilentFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpCilentFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpCilentFactory.CreateClient();
            string url = @"https://localhost:7114/api/Storehouse";
            HttpResponseMessage msg;
            ViewBag.Storehouses = null;
            try
            {
                msg = await client.GetAsync(url);
            }
            catch (Exception ex)
            {
                ViewBag.Msg = $"向StorageAPI 請求失敗\n{ex.Message}";
                return View();
            }
            if (!msg.IsSuccessStatusCode)
            {
                ViewBag.Msg = "向StorageAPI 請求失敗";
                return View();
            }
            ViewBag.Msg = "";
            var json = await msg.Content.ReadAsStringAsync();   
            ViewBag.Storehouses = JsonConvert.DeserializeObject<List<StorehouseDetail>>(json);
            ViewData["IDtoken"] = Request.Cookies["StorageToken"];
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<string> Try()
        {
            using HttpClient client = new HttpClient();
            var msg = await client.GetAsync(@"https://localhost:7114/api/People");
            if(msg.StatusCode == HttpStatusCode.OK)
            {
                var str = await msg.Content.ReadAsStringAsync();
                return str;
            }

            return msg.StatusCode.ToString();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
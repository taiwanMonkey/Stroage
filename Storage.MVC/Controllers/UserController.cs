using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Stroage.API.Models;
using Stroage.API.RequestModels;
using System.Net;
using System.Text.Encodings;

namespace Storage.MVC.Controllers
{
    public class UserController : Controller
    {
        private IHttpClientFactory _httpClientFactory;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(string id, string password)
        {
            using HttpClient client = _httpClientFactory.CreateClient();
            string j = JsonConvert.SerializeObject(new { id, password });
            HttpContent content = new StringContent(j);
            var s = @$"https://localhost:7114/api/People/Login?id={id}&password={password}";
            var result = await client.PostAsync(@$"https://localhost:7114/api/People/Login?id={id}&password={password}", content);
            if(result.StatusCode != System.Net.HttpStatusCode.OK)
                return RedirectToAction("Index");
            var ctn = await result.Content.ReadAsStringAsync();
            var rep = JsonConvert.DeserializeObject<LoginResponse>(ctn);
            Response.Cookies.Append("StorageToken", rep.StorageToken, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddMinutes(30),
                IsEssential = true
            });
            Response.Cookies.Append("S_UserName", rep.UserName, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddMinutes(30)
            });
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register(string hint = "empty")
        {
            if(!string.IsNullOrEmpty(hint))
                ViewBag.Hint = hint;
            return View();
        }

        [HttpPost]
        public async  Task<IActionResult> OnRegister(string id, string name, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
                return BadRequest();
            if (password != confirmPassword)
            {
                var path = "Register?hint=Confirm Password";

                return Redirect(path);
            }
            
            string url = "https://localhost:7114/api/People";
            using var client = _httpClientFactory.CreateClient();
            
            Person person = new Person();
            person.Name = name;
            person.Password = password;
            person.Id = id;
            var msg = await client.PostAsJsonAsync(url, person);
            
            if(!msg.IsSuccessStatusCode)
            {
                return Redirect("Register?hint=ID has been registed");
            }

            return RedirectToAction("Index", "User");

        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("StorageToken");
            Response.Cookies.Delete("S_UserName");
            return RedirectToAction("Index", "Home");
        }
    }
}

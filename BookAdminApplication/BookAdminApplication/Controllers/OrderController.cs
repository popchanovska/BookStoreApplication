using System.Text;
using BookAdminApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookAdminApplication.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:7295/api/orders/GetAllOrders";

            HttpResponseMessage response = client.GetAsync(URL).Result;
            var data = response.Content.ReadAsAsync<List<Order>>().Result;
            return View(data);
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            HttpClient client = new HttpClient();
            string URL = $"https://localhost:7295/api/orders/GetOrder/{id}";

            HttpResponseMessage response = client.GetAsync(URL).Result;

            var result = response.Content.ReadAsAsync<Order>().Result;

            return View(result);
        }
    }
}

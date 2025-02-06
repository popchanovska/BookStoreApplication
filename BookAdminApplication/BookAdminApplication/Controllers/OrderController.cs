using System.Text;
using BookAdminApplication.Models;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookAdminApplication.Controllers
{
    public class OrderController : Controller
    {
        public OrderController()
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            string URL = "http://localhost:5285/api/orders/GetAllOrders";

            HttpResponseMessage response = client.GetAsync(URL).Result;
            var data = response.Content.ReadAsAsync<List<Order>>().Result;
            return View(data);
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            HttpClient client = new HttpClient();
            string URL = $"http://localhost:5285/api/orders/GetOrder/{id}";

            HttpResponseMessage response = client.GetAsync(URL).Result;

            var result = response.Content.ReadAsAsync<Order>().Result;

            return View(result);
        }

        [HttpGet]
        public FileContentResult CreateInvoice(string id)
        {
            HttpClient client = new HttpClient();

            string URL = $"http://localhost:5285/api/orders/GetOrder/{id}";
            var model = new
            {
                Id = id
            };

            HttpResponseMessage response = client.GetAsync(URL).Result;

            var result = response.Content.ReadAsAsync<Order>().Result;

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{OrderNumber}}", result.Id);
            document.Content.Replace("{{Username}}", result.User.UserName);

            StringBuilder sb = new StringBuilder();
            var total = 0d;
            foreach (var item in result.BooksInOrder)
            {
                sb.AppendLine("Book " + item.Book.Title + " has quantity " + item.Quantity + " with price " +
                              item.Book.Price + ".");
                total += (item.Quantity * item.Book.Price);
            }

            document.Content.Replace("{{Books}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", total + "MKD");

            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());
            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }
    }
}
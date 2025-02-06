using BookAdminApplication.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Data;


namespace BookAdminApplication.Controllers
{
    public class OrderController : Controller
    {
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
        public IActionResult Orders()
        {
            HttpClient client = new HttpClient();
            string URL = "http://localhost:5285/api/orders/GetAllOrders";

            HttpResponseMessage response = client.GetAsync(URL).Result;
            var orders = response.Content.ReadAsAsync<List<Order>>().Result;
            var excelBytes = GenerateXlsForOrders(orders);

            return File(excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Orders_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx")
        }

        public byte[] GenerateXlsForOrders(List<Order> orders)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Order ID", typeof(string));
            dataTable.Columns.Add("Order Date", typeof(DateTime));
            dataTable.Columns.Add("Customer Name", typeof(string));
            dataTable.Columns.Add("Number of Books", typeof(int));
            dataTable.Columns.Add("Total Amount", typeof(decimal));

            foreach (var order in orders)
            {
                dataTable.Rows.Add(
                    order.Id,
                    order.OrderDate,
                    order.User?.UserName?? "N/A",
                    order.BooksInOrder.Count(),
                    order.TotalPrice
                );
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(dataTable, "Orders");

                var headerRange = worksheet.Range(1, 1, 1, dataTable.Columns.Count);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightSkyBlue;

                worksheet.Column(2).Style.DateFormat.Format = "dd-MM-yyyy HH:mm:ss";
                worksheet.Column(5).Style.NumberFormat.Format = "ÏÍ‰.#,##0.00";

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
        

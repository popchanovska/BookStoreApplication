using Microsoft.AspNetCore.Mvc;
using BookShopAdmin.Data;
using BookShopAdmin.Models;

namespace BookShopAdmin.Controllers
{
    public class BooksController : Controller
    {

        public BooksController(ApplicationDbContext context)
        {
        }

        public List<Book> FetchBooks()
        {
            List<Book> books = new List<Book>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5285");
            HttpResponseMessage response = client.GetAsync("/api/Books/GetAllBooks").Result;
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                books = response.Content.ReadFromJsonAsync<List<Book>>().Result;
            }
            return books;
            
        }
        public Book FetchBook(Guid id)
        {
            Book book = new Book();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5285");
            HttpResponseMessage response = client.GetAsync($"/api/Books/GetBook/{id}").Result;
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                book = response.Content.ReadAsAsync<Book>().Result;
            }
            return book;
        }   

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var books = FetchBooks();
            return View(books);
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var book = FetchBook(id);
            return View(book);
        }
        
    }
}

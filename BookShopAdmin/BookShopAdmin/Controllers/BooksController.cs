using BookShopAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace BookShopAdmin.Controllers
{
    public class BooksController : Controller
    {

        public BooksController()
        {
        }

        public List<Author> FetchAuthors()
        {
            List<Author> authors = new List<Author>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5285");
            HttpResponseMessage response = client.GetAsync("/api/Authors/GetAllAuthors").Result;
            if (response.IsSuccessStatusCode)
            {
                authors = response.Content.ReadFromJsonAsync<List<Author>>().Result;
            }

            return authors;

        }

        public List<Publisher> FetchPublishers()
        {
            List<Publisher> publishers = new List<Publisher>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5285");
            HttpResponseMessage response = client.GetAsync("/api/Publishers/GetAllPublishers").Result;
            if (response.IsSuccessStatusCode)
            {

                publishers = response.Content.ReadFromJsonAsync<List<Publisher>>().Result;
            }

            return publishers;

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
            if (id == null)
            {
                return NotFound();
            }

            var book = FetchBook(id);

            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var book = FetchBook(id);
            if (book == null)
            {
                throw new InvalidOperationException("Book cannot be null.");
            }

            var authors = FetchAuthors();
            var publishers = FetchPublishers();

            if (authors == null || !authors.Any())
            {
                throw new InvalidOperationException("Authors cannot be null or empty.");
            }

            if (publishers == null || !publishers.Any())
            {
                throw new InvalidOperationException("Publishers cannot be null or empty.");
            }

            // Log or inspect the data
            Console.WriteLine($"Book: {book.Title}, AuthorId: {book.AuthorId}, PublisherId: {book.PublisherId}");
            Console.WriteLine($"Authors count: {authors.Count}, Publishers count: {publishers.Count}");

            // Populate the ViewBag with the authors and publishers
            ViewData["AuthorId"] = new SelectList(authors, "Id", "FullName");
            ViewData["PublisherId"] = new SelectList(publishers, "Id", "Name");

            return View(book);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Book book)
        {
            if (!ModelState.IsValid)
            {
                // Fetch the authors and publishers again if validation fails
                var allAuthors = FetchAuthors();
                var allPublishers = FetchPublishers();

                // Re-populate ViewBag for dropdowns
                ViewData["AuthorId"] = new SelectList(allAuthors, "Id", "FullName");
                ViewData["PublisherId"] = new SelectList(allPublishers, "Id", "Name");

                // Return the view to allow corrections
                return View(book);
            }

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5285");

                HttpResponseMessage response = await client.PutAsJsonAsync($"/api/Books/UpdateBook/{book.Id}", book);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            // If the update failed, re-populate dropdowns
            var authors = FetchAuthors();
            var publishers = FetchPublishers();

            ViewData["AuthorId"] = new SelectList(authors, "Id", "FullName");
            ViewData["PublisherId"] = new SelectList(publishers, "Id", "Name");

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            var authors = FetchAuthors();
            var publishers = FetchPublishers();
            ViewBag.Authors = new SelectList(authors, "Id", "LastName");
            ViewBag.Publishers = new SelectList(publishers, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book) // Accepting a Book parameter
        {
            if (!ModelState.IsValid)
            {
                // Repopulate ViewBag if the model state is invalid
                var authors = FetchAuthors();
                var publishers = FetchPublishers();
                ViewBag.Authors = new SelectList(authors, "Id", "LastName");
                ViewBag.Publishers = new SelectList(publishers, "Id", "Name");
                return View(book); // Pass the model back to the view
            }

            // Create the HttpClient and send the request
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5285");
                HttpResponseMessage response = client.PostAsJsonAsync("/api/Books/CreateBook", book).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Error while creating the book.");
                    // Repopulate ViewBag in case of failure
                    var authors = FetchAuthors();
                    var publishers = FetchPublishers();
                    ViewBag.Authors = new SelectList(authors, "Id", "LastName");
                    ViewBag.Publishers = new SelectList(publishers, "Id", "Name");
                    return View(book); // Return the view with the model
                }
            }
        }


        }

}

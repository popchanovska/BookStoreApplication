using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookApplication.Domain.Domain;
using BookApplication.Service;
using Microsoft.Identity;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BookApplication.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly MainService mainService;
        public BooksController(MainService _mainService)
        {
            mainService = _mainService;
        }
        public IActionResult Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString; 

            var books = mainService.Book.GetAllBooks();

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b =>
                    (b.Title != null && b.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                    (b.ISBN != null && b.ISBN.Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                    (b.Author != null && b.Author.FullName != null &&
                     b.Author.FullName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            return View(books);
        }

        // GET: Books/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = mainService.Book.getDetailsForBook(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(mainService.Author.GetNamesForAuthors(), "Id", "FullName");

            ViewData["PublisherId"] = new SelectList(mainService.Publisher.GetAllPublishers(), "Id", "Name");
            return View();
        }

        // POST: Books/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("AuthorId,Title,ISBN,Price,CoverImage,PublicatonYear,IsHardcover,Rating,Genre,PublisherId,Id")] Book book)
        {
            if (ModelState.IsValid)
            {
                book.Id = Guid.NewGuid();
                mainService.Book.CreateNewBook(book);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(mainService.Author.GetNamesForAuthors(), "Id", "FullName");
            ViewData["PublisherId"] = new SelectList(mainService.Publisher.GetAllPublishers(), "Id", "Name", book.PublisherId);
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = mainService.Book.getDetailsForBook(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(mainService.Author.GetNamesForAuthors(), "Id", "FullName");
            ViewData["PublisherId"] = new SelectList(mainService.Publisher.GetAllPublishers(), "Id", "Name", book.PublisherId);
            return View(book);
        }

        // POST: Books/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("AuthorId,Title,ISBN,Price,CoverImage,PublicatonYear,IsHardcover,Rating,Genre,PublisherId,Id")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    mainService.Book.UpdateExistingBook(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(mainService.Author.GetNamesForAuthors(), "Id", "FullName");
            ViewData["PublisherId"] = new SelectList(mainService.Publisher.GetAllPublishers(), "Id", "Name", book.PublisherId);
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize]
        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = mainService.Book.getDetailsForBook(id);
            if (book == null)
            {
                return NotFound();
            }
            
            return View(book);
        }

        // POST: Books/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var book = mainService.Book.getDetailsForBook(id);
            if (book != null)
            {
                mainService.Book.DeleteBook(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(Guid id)
        {
            return mainService.Book.getDetailsForBook(id) != null;
        }
     
    }

}

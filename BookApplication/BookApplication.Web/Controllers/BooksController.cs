using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookApplication.Domain.Domain;
using BookApplication.Repository;
using BookApplication.Service.Interface;
using BookApplication.Service;

namespace BookApplication.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly MainService mainService;
        public BooksController(MainService _mainService)
        {
            mainService = _mainService;
        }

        // GET: Books
        public IActionResult Index()
        {
            return View(mainService.Book.GetAllBooks());
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
        public IActionResult Create()
        {
            // ViewData["AuthorId"] = new SelectList(_authorService.GetNamesForAuthors(), "Id", _authorService.GetNamesForAuthors().ToString());
            ViewData["AuthorId"] = new SelectList(mainService.Author.GetNamesForAuthors(), "Id", "FullName");

            ViewData["PublisherId"] = new SelectList(mainService.Publisher.GetAllPublishers(), "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

            mainService.Book.DeleteBook(id);

            return View(book);
        }

        // POST: Books/Delete/5
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

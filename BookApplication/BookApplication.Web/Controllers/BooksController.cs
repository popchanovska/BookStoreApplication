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

namespace BookApplication.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly IPublisherService _publisherService;
        public BooksController(IBookService bookService, IAuthorService authorService, IPublisherService publisherService)
        {
            _bookService = bookService;
            _authorService = authorService;
            _publisherService = publisherService;   
        }

        // GET: Books
        public IActionResult Index()
        {
            return View(_bookService.GetAllBooks());
        }

        // GET: Books/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _bookService.getDetailsForBook(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_authorService.GetAllAuthors(), "Id", "Biography");


            ViewData["PublisherId"] = new SelectList(_publisherService.GetAllPublishers(), "Id", "Email");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorId,Title,ISBN,Price,CoverImage,PublicatonYear,IsHardcover,Rating,Genre,PublisherId,Id")] Book book)
        {
            if (ModelState.IsValid)
            {
                book.Id = Guid.NewGuid();
                _bookService.CreateNewBook(book);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_authorService.GetAllAuthors(), "Id", "Biography", book.AuthorId);
            ViewData["PublisherId"] = new SelectList(_publisherService.GetAllPublishers(), "Id", "Email", book.PublisherId);
            return View(book);
        }

        // GET: Books/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _bookService.getDetailsForBook(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_authorService.GetAllAuthors(), "Id", "Biography", book.Id);
            ViewData["PublisherId"] = new SelectList(_publisherService.GetAllPublishers(), "Id", "Email", book.PublisherId);
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
                    _bookService.UpdateExistingBook(book);
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
            ViewData["AuthorId"] = new SelectList(_authorService.GetAllAuthors(), "Id", "Biography", book.AuthorId);
            ViewData["PublisherId"] = new SelectList(_publisherService.GetAllPublishers(), "Id", "Email", book.PublisherId);
            return View(book);
        }

        // GET: Books/Delete/5
        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _bookService.getDetailsForBook(id);
            if (book == null)
            {
                return NotFound();
            }

            _bookService.DeleteBook(id);

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var book = _bookService.getDetailsForBook(id);
            if (book != null)
            {
                _bookService.DeleteBook(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(Guid id)
        {
            return _bookService.getDetailsForBook(id)!=null;
        }
    }
}

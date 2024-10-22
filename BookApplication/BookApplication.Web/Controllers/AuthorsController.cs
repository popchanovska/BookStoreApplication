using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookApplication.Domain.Domain;
using BookApplication.Service;

namespace BookApplication.Web.Controllers
{
    public class AuthorsController : Controller
    {
      //  private readonly IAuthorService _authorService;
        private readonly MainService mainService;

        public AuthorsController(MainService _mainService)
        {
            mainService = _mainService;
        }

        // GET: Authors
        public IActionResult Index()
        {
            return View(mainService.Author.GetAllAuthors());
        }

        // GET: Authors/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = mainService.Author.getDetailsForAuthor(id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstName,LastName,Biography,DateOfBirth,Image,Id")] Author author)
        {
            if (ModelState.IsValid)
            {
                author.Id = Guid.NewGuid();
                mainService.Author.CreateNewAuthor(author);
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = mainService.Author.getDetailsForAuthor(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("FirstName,LastName,Biography,DateOfBirth,Image,Id")] Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    mainService.Author.UpdateExistingAuthor(author);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.Id))
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
            return View(author);
        }

        // GET: Authors/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = mainService.Author.getDetailsForAuthor(id) as Author;
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            if (id != null)
            {
                mainService.Author.DeleteAuthor(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(Guid id)
        {
            return mainService.Author.getDetailsForAuthor(id) != null;
        }
    }
}

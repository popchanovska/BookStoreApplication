using BookApplication.Domain.Domain;
using BookApplication.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookApplication.Web.Controllers.api
{
    //[Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly MainService _mainService;

        public AdminController(MainService mainService)
        {
            _mainService = mainService;
        }

        // GET: api/Books/GetAllBooks
        [HttpGet]
        [Route("api/Books/GetAllBooks")]
        public IActionResult GetAllBooks()
        {
            var books = _mainService.Book.GetAllBooks().ToList();
            //foreach (var book in books)
            //{
            //    book.CoverImage = "";
            //    book.Author.Image = "";
            //}
            return Ok(books);
        }

        // GET: api/Authors/GetAllAuthors
        [HttpGet]
        [Route("api/Authors/GetAllAuthors")]
        public IActionResult GetAllAuthors()
        {
            var authors = _mainService.Author.GetAllAuthors().ToList();
            return Ok(authors);
        }


        // GET: api/Publishers/GetAllPublishers
        [HttpGet]
        [Route("api/Publishers/GetAllPublishers")]
        public IActionResult GetAllPublishers()
        {
            var publishers = _mainService.Publisher.GetAllPublishers().ToList();
            return Ok(publishers);
        }

        // GET: api/Books/GetBook/{id}
        [HttpGet]
        [Route("api/Books/GetBook/{id}")]
        public IActionResult GetBook(Guid id)
        {
            var book = _mainService.Book.getDetailsForBook(id);
            return Ok(book);
        }

        // POST: api/Books/EditBook
        [HttpPut]
        [Route("api/Books/EditBook")]
        public IActionResult EditBook([FromBody] Book book)
        {
            _mainService.Book.UpdateExistingBook(book);
            return Ok();
        }

        // POST: api/Books/CreateBook
        [HttpPost]
        [Route("api/Books/CreateBook")]
        public IActionResult CreateBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            _mainService.Book.CreateNewBook(book);
            return Ok();

        }
    }
}

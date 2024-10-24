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
        private readonly MainService mainService;

        public AdminController(MainService _mainService)
        {
            mainService = _mainService;
        }

        // GET: api/Books/GetAllBooks
        [HttpGet]
        [Route("api/Books/GetAllBooks")]
        public IActionResult GetAllBooks()
        {
            var books = mainService.Book.GetAllBooks().ToList();
            //foreach (var book in books)
            //{
            //    book.CoverImage = "";
            //    book.Author.Image = "";
            //}
            return Ok(books); // Use Ok() to ensure a proper JSON response
        }

        // GET: api/Authors/GetAllAuthors
        [HttpGet]
        [Route("api/Authors/GetAllAuthors")]
        public IActionResult GetAllAuthors()
        {
            var authors = mainService.Author.GetAllAuthors().ToList();
            return Ok(authors); // Use Ok() to ensure a proper JSON response
        }


        // GET: api/Publishers/GetAllPublishers
        [HttpGet]
        [Route("api/Publishers/GetAllPublishers")]
        public IActionResult GetAllPublishers()
        {
            var publishers = mainService.Publisher.GetAllPublishers().ToList();
            return Ok(publishers); // Use Ok() to ensure a proper JSON response
        }

        // GET: api/Books/GetBook/{id}
        [HttpGet]
        [Route("api/Books/GetBook/{id}")]
        public IActionResult GetBook(Guid id)
        {
            var book = mainService.Book.getDetailsForBook(id);
            return Ok(book); // Use Ok() to ensure a proper JSON response
        }

        // POST: api/Books/EditBook
        [HttpPut]
        [Route("api/Books/EditBook")]
        public IActionResult EditBook([FromBody] Book book)
        {
            mainService.Book.UpdateExistingBook(book);
            return Ok(); // Use Ok() to ensure a proper JSON response
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
            mainService.Book.CreateNewBook(book);
            return Ok(); // Use Ok() to ensure a proper JSON response

        }




    }
}

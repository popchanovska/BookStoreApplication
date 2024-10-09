using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BookApplication.Domain.Domain;
using BookApplication.Repository;
using BookApplication.Service.Implementation;
using BookApplication.Service.Interface;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BookApplication.Web.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IShoppingCartsService _shoppingCartService;
        private readonly IBookInShoppingCart _bookInShoppingCartService;
        private readonly IBookService _bookService;

        public ShoppingCartsController(IShoppingCartsService shoppingCartService, ApplicationDbContext context, IBookInShoppingCart bookInShoppingCartService, IBookService bookService)
        {
            _shoppingCartService = shoppingCartService;
            _context = context;
            _bookService = bookService; 
            _bookInShoppingCartService = bookInShoppingCartService;
        }

        // GET: ShoppingCarts
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(User.Identity.IsAuthenticated == true)
                return View(_shoppingCartService.GetAllShoppingCartsForUser(userId));
            return Unauthorized();
        }

        // GET: ShoppingCarts/Details/5
        public IActionResult Details(Guid? id)
        {
            if(User.Identity.IsAuthenticated == true) { 
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = _shoppingCartService.GetDetailsForShoppingCart(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
            }
            return Unauthorized();
        }

        // GET: ShoppingCarts/Create
        public IActionResult Create()
        {
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ShoppingCarts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("OwnerId,Id")] ShoppingCart shoppingCart)
        {
            if(User.Identity.IsAuthenticated == true)
            {
                if (ModelState.IsValid)
                {
                    shoppingCart.Id = Guid.NewGuid();
                    _shoppingCartService.CreateNewShoppingCart(shoppingCart);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", shoppingCart.OwnerId);
                return View(shoppingCart);
            }
           
            return Unauthorized();
        }
          

        // GET: ShoppingCarts/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = _shoppingCartService.GetDetailsForShoppingCart(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", shoppingCart.OwnerId);
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("OwnerId,Id")] ShoppingCart shoppingCart)
        {
            if (id != shoppingCart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _shoppingCartService.UpdateExistingShoppingCart(shoppingCart);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartExists(shoppingCart.Id))
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
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", shoppingCart.OwnerId);
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = _shoppingCartService.GetDetailsForShoppingCart(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var shoppingCart = _shoppingCartService.GetDetailsForShoppingCart(id);
            if (shoppingCart != null)
            {
                _shoppingCartService.DeleteShoppingCart(id);
            }

            return RedirectToAction(nameof(Index));
        }
        private bool ShoppingCartExists(Guid id)
        {
            return _shoppingCartService.GetDetailsForShoppingCart(id) != null;
        }
        //public void AddToCart(Guid? Id)
        //{
        //    // Check if Id is null
        //    if (!Id.HasValue)
        //    {
        //        throw new ArgumentNullException("Id", "Book Id is null");
        //    }

        //    Console.WriteLine("Book Id: " + Id);

        //    // Get book details
        //    var book = _bookService.getDetailsForBook(Id.Value);
        //    if (book == null)
        //    {
        //        throw new Exception($"Book with Id {Id.Value} not found");
        //    }

        //    // Get user
        //    var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (string.IsNullOrEmpty(user))
        //    {
        //        throw new Exception("User not authenticated");
        //    }

        //    // Get user's shopping cart
        //    var shoppingCart = _shoppingCartService.GetAllShoppingCartsForUser(user).FirstOrDefault();
        //    if (shoppingCart == null)
        //    {
        //        throw new Exception("No shopping cart found for the user");
        //    }

        //    // Get shopping cart details
        //    ShoppingCart cartDetails = _shoppingCartService.GetDetailsForShoppingCart(shoppingCart.Id);
        //    if (cartDetails == null)
        //    {
        //        throw new Exception($"No details found for shopping cart with Id {shoppingCart.Id}");
        //    }

        //    // Add book to shopping cart
        //    BookInShoppingCart bookInShoppingCart = new BookInShoppingCart
        //    {
        //        Book = book,
        //        BookId = book.Id,
        //        ShoppingCart = cartDetails,
        //        ShoppingCartId = cartDetails.Id,
        //        Quantity = 1
        //    };

        //    _bookInShoppingCartService.AddBookToShoppingCart(bookInShoppingCart);

        //    // Optionally log the quantity of books in the cart
        //    var bookInCart = _bookInShoppingCartService.GetAllBooksInShoppingCart(shoppingCart.Id).FirstOrDefault();
        //    if (bookInCart != null)
        //    {
        //        Console.WriteLine("Quantity: " + bookInCart.Quantity);
        //    }
        //    else
        //    {
        //        Console.WriteLine("No books found in shopping cart");
        //    }
        


        public IActionResult AddToCart (Guid Id)
        {
            var book = _bookService.getDetailsForBook(Id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var shoppingCart = _shoppingCartService.GetAllShoppingCartsForUser(userId).FirstOrDefault();



            BookInShoppingCart bookInShoppingCart = new BookInShoppingCart
            { 
                Id = Guid.NewGuid(),
                Book = book,
                BookId = book.Id,
                ShoppingCart = shoppingCart,
                ShoppingCartId = shoppingCart.Id,
                Quantity = 1
            };
            
            if(bookInShoppingCart != null) {
                _bookInShoppingCartService.AddBookToShoppingCart(bookInShoppingCart);
                Console.Write("SUCCESS: Book added to cart");
            }

            return RedirectToAction("Index");
        }

    }


}

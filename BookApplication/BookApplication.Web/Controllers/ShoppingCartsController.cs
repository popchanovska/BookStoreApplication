using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BookApplication.Domain.Domain;
using BookApplication.Repository;
using BookApplication.Service;

namespace BookApplication.Web.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly MainService mainService;


        public ShoppingCartsController(MainService _mainService, ApplicationDbContext context)
        {
            mainService = _mainService;
            _context = context;
        }

        // GET: ShoppingCarts
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid shpId = mainService.ShoppingCart.GetAllShoppingCartsForUser(userId).FirstOrDefault().Id;
            var booksInShp = mainService.BookInShoppingCart.GetAllBooksInShoppingCart(shpId);
            ViewBag.BookInShoppingCart = booksInShp;
            ViewBag.TotalPrice = booksInShp.Select(x => x.Book.Price * x.Quantity).Sum();
            ViewBag.ShoppingCartId = shpId;

            if (User.Identity.IsAuthenticated == true)
                return View(mainService.ShoppingCart.GetAllShoppingCartsForUser(userId));
            return Unauthorized();
        }

        // GET: ShoppingCarts/Details/5
        public IActionResult Details(Guid? id)
        {
            if (User.Identity.IsAuthenticated == true)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var shoppingCart = mainService.ShoppingCart.GetDetailsForShoppingCart(id);
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
            if (User.Identity.IsAuthenticated == true)
            {
                if (ModelState.IsValid)
                {
                    shoppingCart.Id = Guid.NewGuid();
                    mainService.ShoppingCart.CreateNewShoppingCart(shoppingCart);
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

            var shoppingCart = mainService.ShoppingCart.GetDetailsForShoppingCart(id);
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
                    mainService.ShoppingCart.UpdateExistingShoppingCart(shoppingCart);
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

        private bool ShoppingCartExists(Guid id)
        {
            return mainService.ShoppingCart.GetDetailsForShoppingCart(id) != null;
        }

        // GET: ShoppingCarts/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = mainService.ShoppingCart.GetDetailsForShoppingCart(id);
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
            var shoppingCart = mainService.ShoppingCart.GetDetailsForShoppingCart(id);
            if (shoppingCart != null)
            {
                mainService.ShoppingCart.DeleteShoppingCart(id);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult AddToCart(Guid BookId, int bookQuantity)
        {
            var book = mainService.Book.getDetailsForBook(BookId);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var shoppingCart = mainService.ShoppingCart.GetAllShoppingCartsForUser(userId).FirstOrDefault();

            if (book == null || shoppingCart == null)
            {
                return NotFound();
            }

            BookInShoppingCart bookInShoppingCart = new BookInShoppingCart
            {
                Id = Guid.NewGuid(),
                Book = book,
                BookId = book.Id,
                ShoppingCart = shoppingCart,
                ShoppingCartId = shoppingCart.Id,
                Quantity = bookQuantity
            };

            if (bookInShoppingCart != null)
            {
                mainService.BookInShoppingCart.AddBookToShoppingCart(bookInShoppingCart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult RemoveBookFromCart(Guid Id)
        {
            var bsc = mainService.BookInShoppingCart.GetBookInShoppingCart(Id);
            if (bsc != null)
            {
                mainService.BookInShoppingCart.DeleteBookFromShoppingCart(bsc);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditBookQuantity(Guid BookInScId, int BookQuantity)
        {
            var book = mainService.BookInShoppingCart.GetBookInShoppingCart(BookInScId);
            if (book != null)
            {
                book.Quantity = BookQuantity;
                mainService.BookInShoppingCart.EditBookInShoppingCart(book);
            }

            return RedirectToAction("Index");
        }

        private Address CreateAddress(Address address)
        {
            if (address != null)
            {
                mainService.Address.CreateAddress(address);
                return address;
            }

            return null;
        }

        private List<BookInOrder> CreateBookInOrder(List<BookInShoppingCart> bookInShoppingCarts, Guid orderId)
        {
            List<BookInOrder> booksInOrder = new List<BookInOrder>();

            foreach (var book in bookInShoppingCarts)
            {
                var bookInOrder = new BookInOrder()
                {
                    Id = new Guid(),
                    OrderId = orderId,
                    BookId = book.BookId,
                    Quantity = book.Quantity,
                };
                mainService.BookInOrder.AddBookInOrder(bookInOrder);
                booksInOrder.Add(bookInOrder);
            }

            return booksInOrder;
        }

        [HttpPost]
        public IActionResult CreateOrder(Guid Id, [Bind("Street,City,Country,ZipCode")] Address addr, Double totalPrice)
        {
            if (ModelState.IsValid)
            {
                var shoppingCart = mainService.ShoppingCart.GetDetailsForShoppingCart(Id);
                if (shoppingCart != null)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user = _context.Users.FirstOrDefault(x => x.Id == userId);
                    addr.Id = Guid.NewGuid();
                    var address = CreateAddress(addr);
                    if (user != null && address != null)
                    {
                        Guid orderId = Guid.NewGuid();
                        Order order = new Order
                        {
                            Id = orderId,
                            Address = address,
                            AddressId = address.Id,
                            User = user,
                            UserId = user.Id,
                            shoppingCart = shoppingCart,
                            TotalPrice = totalPrice,
                            // BooksInOrder = CreateBookInOrder(mainService.BookInShoppingCart.GetAllBooksInShoppingCart(Id),orderId)
                        };

                        mainService.Order.AddNewOrder(order);
                        order.BooksInOrder =
                            CreateBookInOrder(mainService.BookInShoppingCart.GetAllBooksInShoppingCart(Id), orderId);
                        mainService.Order.UpdateOrder(order);
                        mainService.BookInShoppingCart.EmptyCart(Id);
                        return RedirectToAction("Success", new { orderId = order.Id });
                    }
                }

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Success(Guid orderId)
        {
            BaseEntity Id = new BaseEntity
            {
                Id = orderId
            };
            var order = mainService.Order.GetDetailsForOrder(Id);

            var shpId = mainService.Order.GetShoppingCartId(Id);
            ViewBag.ShoppingCartId = shpId;
            ViewBag.TotalPrice = mainService.Order.GetDetailsForOrder(Id).TotalPrice;
            var booksInShp = mainService.BookInShoppingCart.GetAllBooksInShoppingCart(shpId);
            ViewBag.BooksInShoppingCart = booksInShp;

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
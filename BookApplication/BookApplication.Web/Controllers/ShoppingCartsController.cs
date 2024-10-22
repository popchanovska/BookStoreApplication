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
using BookApplication.Repository.Implementation;
using BookApplication.Service.Implementation;
using BookApplication.Service.Interface;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace BookApplication.Web.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IShoppingCartsService _shoppingCartService;
        private readonly IBookInShoppingCart _bookInShoppingCartService;
        private readonly IBookService _bookService;
        private readonly IOrderService _orderService;
        private readonly IAddressService _addressService;
        private readonly IBookInOrderService _bookInOrderService;
      
            
        public ShoppingCartsController(IShoppingCartsService shoppingCartService, ApplicationDbContext context, IBookInShoppingCart bookInShoppingCartService, IBookService bookService,IOrderService orderService,IAddressService addressService, IBookInOrderService bookInOrderService)
        {
            _shoppingCartService = shoppingCartService;
            _context = context;
            _bookService = bookService;
            _bookInShoppingCartService = bookInShoppingCartService;
            _orderService = orderService;
            _addressService = addressService;
            _bookInOrderService = bookInOrderService;
            
        }

        // GET: ShoppingCarts
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid shpId = _shoppingCartService.GetAllShoppingCartsForUser(userId).FirstOrDefault().Id;
            var booksInShp = _bookInShoppingCartService.GetAllBooksInShoppingCart(shpId);
            ViewBag.BookInShoppingCart = booksInShp;
            ViewBag.TotalPrice = booksInShp.Select(x => x.Book.Price * x.Quantity).Sum();
            ViewBag.ShoppingCartId = shpId;

            if (User.Identity.IsAuthenticated == true)
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


        [HttpPost]
        public IActionResult AddToCart (Guid BookId, int bookQuantity)
        {
            var book = _bookService.getDetailsForBook(BookId);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var shoppingCart = _shoppingCartService.GetAllShoppingCartsForUser(userId).FirstOrDefault();

            if(book == null || shoppingCart == null)
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
            
            if(bookInShoppingCart != null) {
                _bookInShoppingCartService.AddBookToShoppingCart(bookInShoppingCart);
            }

            return RedirectToAction("Index");
        }
        public IActionResult RemoveBookFromCart(Guid Id) {        
            
            var bsc = _bookInShoppingCartService.GetBookInShoppingCart(Id);
            if(bsc != null)
            {
                _bookInShoppingCartService.DeleteBookFromShoppingCart(bsc);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditBookQuantity(Guid BookInScId, int BookQuantity)
        {
            var book = _bookInShoppingCartService.GetBookInShoppingCart(BookInScId);
            if (book != null)
            {
                book.Quantity = BookQuantity;
                _bookInShoppingCartService.EditBookInShoppingCart(book);
            }

            return RedirectToAction("Index");
        }

        private Address CreateAddress(Address address)
        {

            if (address != null)
            { 
                _addressService.CreateAddress(address);
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
                _bookInOrderService.AddBookInOrder(bookInOrder);
                booksInOrder.Add(bookInOrder);
            }
            
            return booksInOrder;
        }
        
        [HttpPost]
        public IActionResult CreateOrder(Guid Id, [Bind("Street,City,Country,ZipCode")] Address addr, Double totalPrice)
        {
            if (ModelState.IsValid)
            {
                var shoppingCart = _shoppingCartService.GetDetailsForShoppingCart(Id);
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
                            // BooksInOrder = CreateBookInOrder(_bookInShoppingCartService.GetAllBooksInShoppingCart(Id),orderId)
                        };

                        _orderService.AddNewOrder(order);
                        order.BooksInOrder = CreateBookInOrder(_bookInShoppingCartService.GetAllBooksInShoppingCart(Id), orderId);
                        _orderService.UpdateOrder(order);
                        _bookInShoppingCartService.EmptyCart(Id);
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
                var order = _orderService.GetDetailsForOrder(Id);

            var shpId = _orderService.GetDetailsForOrder(Id).shoppingCart.Id;
            ViewBag.ShoppingCartId = shpId;
            ViewBag.TotalPrice = _orderService.GetDetailsForOrder(Id).TotalPrice;
            var booksInShp = _bookInShoppingCartService.GetAllBooksInShoppingCart(shpId);
            ViewBag.BooksInShoppingCart = booksInShp;

            if (order == null)
                {
                    return NotFound(); 
                }

                return View(order); 
            }

        }


    }

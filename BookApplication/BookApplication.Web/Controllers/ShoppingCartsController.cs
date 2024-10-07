using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookApplication.Domain.Domain;
using BookApplication.Repository;
using BookApplication.Service.Implementation;
using BookApplication.Service.Interface;

namespace BookApplication.Web.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IShoppingCartsService _shoppingCartService;

        public ShoppingCartsController(IShoppingCartsService shoppingCartService, ApplicationDbContext context)
        {
            _shoppingCartService= shoppingCartService;
            _context = context;
        }

        // GET: ShoppingCarts
        public async Task<IActionResult> Index()
        {
            return View(_shoppingCartService.GetAllShoppingCarts());
        }

        // GET: ShoppingCarts/Details/5
        public async Task<IActionResult> Details(Guid? id)
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
            if (ModelState.IsValid)
            {
                shoppingCart.Id = Guid.NewGuid();
                _shoppingCartService.CreateNewShoppingCart(shoppingCart);
                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", shoppingCart.OwnerId);
            return View(shoppingCart);
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
    }
}

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
    public class PublishersController : Controller
    {

        private readonly IPublisherService _publisherService;
        private readonly IAddressService _addressService;
        public PublishersController(IPublisherService publisherService, IAddressService addressService)
        {
            _publisherService=publisherService;
            _addressService = addressService;
        }

        // GET: Publishers
        public IActionResult Index()
        {
            ViewData["AddressId"] = new SelectList(_addressService.GetAllAddresses(), "Id", "City");
            return View(_publisherService.GetAllPublishers());

        }

        // GET: Publishers/Details/5
        public IActionResult Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = _publisherService.GetPublisher(id);
            {
                return NotFound();
            }

            return View(publisher);
        }

        // GET: Publishers/Create
        public IActionResult Create()
        {
            // ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "City");
            return View();
        }

        // POST: Publishers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Email,Website,PhoneNumber,Address")] Publisher publisher)
        {
            // Ensure Address is not null
            if (publisher.Address == null)
            {
                publisher.Address = new Address();
            }

            if (ModelState.IsValid)
            {
                publisher.Id = Guid.NewGuid();

                // Save the Address entity
                _addressService.CreateAddress(publisher.Address);

                // Associate the AddressId with the Publisher and save Publisher
                publisher.AddressId = publisher.Address.Id;  // Ensure Address has an ID (set by database or manually)
                _publisherService.CreatePublisher(publisher);

                return RedirectToAction(nameof(Index));
            }

            return View(publisher);
        }

        // GET: Publishers/Edit/5
        public IActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = _publisherService.GetPublisher(id);
            if (publisher == null)
            {
                return NotFound();
            }
           // ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "City", publisher.AddressId);
            return View(publisher);
        }

        // POST: Publishers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Name,Email,Website,PhoneNumber,AddressId,Id")] Publisher publisher)
        {
            if (id != publisher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _publisherService.UpdatePublisher(publisher);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublisherExists(publisher.Id))
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
           // ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "City", publisher.AddressId);
            return View(publisher);
        }

        // GET: Publishers/Delete/5
        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = _publisherService.GetPublisher(id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // POST: Publishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var publisher = _publisherService.GetPublisher(id);
            if (publisher != null)
            {
                _publisherService.DeletePublisher(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PublisherExists(Guid id)
        {
            return _publisherService.GetPublisher(id)!=null;
        }
    }
}

﻿using System;
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
        public PublishersController(IPublisherService publisherService)
        {
            _publisherService=publisherService;
        }

        // GET: Publishers
        public IActionResult Index()
        {
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
        public IActionResult Create([Bind("Name,Email,Website,PhoneNumber,AddressId,Id")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                publisher.Id = Guid.NewGuid();
                _publisherService.CreatePublisher(publisher);
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "City", publisher.AddressId);
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
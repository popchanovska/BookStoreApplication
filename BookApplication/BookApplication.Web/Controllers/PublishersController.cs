using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookApplication.Domain.Domain;
using BookApplication.Service;
using Microsoft.AspNetCore.Authorization;


namespace BookApplication.Web.Controllers
{
    public class PublishersController : Controller
    {

        private readonly MainService mainService;
        public PublishersController(MainService _mainService)
        {
            mainService = _mainService;
        }

        // GET: Publishers
        public IActionResult Index()
        {
            ViewData["AddressId"] = new SelectList(mainService.Address.GetAllAddresses(), "Id", "City");
            return View(mainService.Publisher.GetAllPublishers());

        }

        // GET: Publishers/Details/5
        public IActionResult Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = mainService.Publisher.GetPublisher(id);
            if(publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // GET: Publishers/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Publishers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]

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

                mainService.Address.CreateAddress(publisher.Address);
                publisher.AddressId = publisher.Address.Id; 
                mainService.Publisher.CreatePublisher(publisher);

                return RedirectToAction(nameof(Index));
            }

            return View(publisher);
        }

        // GET: Publishers/Edit/5
        [Authorize  ]

        public IActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = mainService.Publisher.GetPublisher(id);
            if (publisher == null)
            {
                return NotFound();
            }
            return View(publisher);
        }

        // POST: Publishers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
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
                    mainService.Publisher.UpdatePublisher(publisher);
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
            return View(publisher);
        }
        [Authorize]

        // GET: Publishers/Delete/5
        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = mainService.Publisher.GetPublisher(id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }
        [Authorize]

        // POST: Publishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var publisher = mainService.Publisher.GetPublisher(id);
            if (publisher != null)
            {
                mainService.Publisher.DeletePublisher(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PublisherExists(Guid id)
        {
            return mainService.Publisher.GetPublisher(id) != null;
        }
    }
}

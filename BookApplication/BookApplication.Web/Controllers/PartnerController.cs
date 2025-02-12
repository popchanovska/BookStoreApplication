using BookApplication.Domain.PartnerModels;
using BookApplication.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookApplication.Web.Controllers
{
    public class PartnerController : Controller
    {
        private readonly ApplicationDbContextPartner _partnerContext;

        public PartnerController(ApplicationDbContextPartner partnerContext)
        {
            _partnerContext = partnerContext;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContextPartner = _partnerContext.Products.Include(p => p.Author).Include(p => p.Category).Include(p => p.Cover);
            return View(await applicationDbContextPartner.ToListAsync());
        }


    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApplication.Domain.PartnerModels;
using Books.Domain.Entities;
namespace BookApplication.Repository
{
    public class ApplicationDbContextPartner : DbContext
    {
        public ApplicationDbContextPartner(DbContextOptions<ApplicationDbContextPartner> options) : base(options) {}

        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }




    

    }
}

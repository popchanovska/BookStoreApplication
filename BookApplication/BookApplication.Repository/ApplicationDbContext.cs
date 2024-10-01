using BookApplication.Domain.Domain;
using BookApplication.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookApplication.Repository
{
    public class ApplicationDbContext : IdentityDbContext<BookAppUser>
    {
        public virtual DbSet<Book> Books { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
    }
}

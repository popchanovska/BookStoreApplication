using BookApplication.Domain.Domain;
using BookApplication.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookApplication.Repository
{
    public class ApplicationDbContext : IdentityDbContext<BookAppUser>
    {
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<BookInShoppingCart> BookInShoppingCarts { get; set; }
        public virtual DbSet<BookInOrder> BookInOrders { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships for Order and BookInOrder
            modelBuilder.Entity<BookInOrder>()
                .HasOne(bio => bio.Order)
                .WithMany(order => order.BooksInOrder)
                .HasForeignKey(bio => bio.OrderId)
                .OnDelete(DeleteBehavior.Restrict); // Change to Restrict or SetNull

            modelBuilder.Entity<BookInOrder>()
                .HasOne(bio => bio.Book)
                .WithMany() // If there are other related entities, configure them too
                .HasForeignKey(bio => bio.BookId)
                .OnDelete(DeleteBehavior.Restrict); // Adjust as needed
        }


    }

}
    
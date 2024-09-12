using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApplication.Domain.Identity;

namespace BookApplication.Domain.Domain
{
    public class ShoppingCart : BaseEntity
    {
        public string? OwnerId { get; set; }
        public BookAppUser? Owner { get; set; }
        public virtual ICollection<BookInShoppingCart>? BooksInShoppingCart { get; set; }
    }
}

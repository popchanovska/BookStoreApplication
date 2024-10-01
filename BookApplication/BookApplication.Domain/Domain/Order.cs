using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApplication.Domain.Identity;

namespace BookApplication.Domain.Domain
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public BookAppUser User { get; set; }
        public string? Address { get; set; }
        public double TotalPrice { get; set; }
        public IEnumerable<BookInOrder> BooksInOrder { get; set; }
    }
}

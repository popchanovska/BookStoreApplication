//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BookApplication.Domain.Identity;

//namespace BookShopAdmin.Models;

//public class Order : BaseEntity
//{
//    public string UserId { get; set; }
//    public DateTime OrderDate { get; set; } = DateTime.Now;
//    public BookAppUser User { get; set; }
//    public Guid AddressId { get; set; }
//    public Address? Address { get; set; }
//    public double TotalPrice { get; set; }
//    public IEnumerable<BookInOrder>? BooksInOrder { get; set; }
//    public ShoppingCart shoppingCart { get; set; }


//}


namespace BookAdminApplication.Models;

public class Order
{
    public string UserId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public BookAppUser User { get; set; }
    public Guid AddressId { get; set; }
    public Address? Address { get; set; }
    public double TotalPrice { get; set; }
    public IEnumerable<BookInOrder>? BooksInOrder { get; set; }
    public ShoppingCart shoppingCart { get; set; }
}
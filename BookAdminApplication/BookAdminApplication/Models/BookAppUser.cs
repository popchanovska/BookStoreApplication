namespace BookAdminApplication.Models;

public class BookAppUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public virtual ShoppingCart ShoppingCart { get; set; } = new ShoppingCart();
    public virtual ICollection<Order>? Order { get; set; }
}
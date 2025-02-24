namespace BookAdminApplication.Models;

public class BookInShoppingCart
{
    public Guid BookId { get; set; }
    public Book? Book { get; set; }
    public Guid ShoppingCartId { get; set; }
    public ShoppingCart? ShoppingCart { get; set; }
    public int Quantity { get; set; }
}
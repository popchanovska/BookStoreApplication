namespace BookAdminApplication.Models;

public class ShoppingCart
{
    public string? OwnerId { get; set; }
    public BookAppUser? Owner { get; set; }
    public virtual ICollection<BookInShoppingCart>? BooksInShoppingCart { get; set; }
}
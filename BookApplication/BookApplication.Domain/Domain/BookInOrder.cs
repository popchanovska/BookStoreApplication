namespace BookApplication.Domain.Domain;

public class BookInOrder : BaseEntity
{
    public Guid BookId { get; set; }
    public Book? Book { get; set; }
    public Guid OrderId { get; set; }
    public int Quantity { get; set; }
}
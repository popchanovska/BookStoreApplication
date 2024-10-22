using BookApplication.Domain.Domain;

namespace BookApplication.Service.Interface;

public interface IBookInOrderService
{
    void AddBookInOrder(BookInOrder bookInOrder);
    List<BookInOrder> GetAllBooksInOrder(Guid? id);
    BookInOrder GetBookInOrder(Guid OrderId, Guid BookId);
}
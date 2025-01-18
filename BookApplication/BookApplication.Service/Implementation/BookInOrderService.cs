using BookApplication.Domain.Domain;
using BookApplication.Repository.Interface;
using BookApplication.Service.Interface;

namespace BookApplication.Service.Implementation;

public class BookInOrderService : IBookInOrderService
{
    private readonly IRepository<BookInOrder> _bookInOrderRepository;
    private readonly IRepository<Order> _orderRepository;

    public BookInOrderService(IRepository<BookInOrder> repository, IRepository<Order> orderRepository)
    {
        _bookInOrderRepository = repository;
        _orderRepository = orderRepository;
    }

    public void AddBookInOrder(BookInOrder bookInOrder)
    {
        _bookInOrderRepository.Insert(bookInOrder);
    }

    public List<BookInOrder> GetAllBooksInOrder(Guid? id)
    {
        var order = _orderRepository.Get(id);
        var booksInOrder = order?.BooksInOrder?.ToList();
        return booksInOrder;
    }

    public BookInOrder GetBookInOrder(Guid OrderId, Guid BookId)
    {
        return GetAllBooksInOrder(OrderId).FirstOrDefault(x => x.BookId == BookId)!;
    }
}
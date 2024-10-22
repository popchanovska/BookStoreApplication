using BookApplication.Domain.Domain;
using BookApplication.Repository.Interface;
using BookApplication.Service.Interface;

namespace BookApplication.Service.Implementation;

public class BookInOrderService : IBookInOrderService
{
    private readonly IRepository<BookInOrder> _bookInOrderRepository;
    private readonly IRepository<Book> _bookRepository;
    private readonly IRepository<Order> _orderRepository;

    public BookInOrderService(IRepository<BookInOrder> repository, IRepository<Order> orderRepository, IRepository<Book> bookRepository)
    {
        _bookInOrderRepository = repository;
        _orderRepository = orderRepository;
        _bookRepository = bookRepository;
    }

    public void AddBookInOrder(BookInOrder bookInOrder)
    {
        _bookInOrderRepository.Insert(bookInOrder);
    }

    public List<BookInOrder> GetAllBooksInOrder(Guid? id)
    {
        return _orderRepository.Get(id).BooksInOrder.ToList();
    }

    public BookInOrder GetBookInOrder(Guid OrderId, Guid BookId)
    {
        return GetAllBooksInOrder(OrderId).FirstOrDefault(x => x.BookId == BookId)!;
    }
}
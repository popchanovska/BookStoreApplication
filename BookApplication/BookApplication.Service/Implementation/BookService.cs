using BookApplication.Domain.Domain;
using BookApplication.Repository.Interface;
using BookApplication.Service.Interface;

namespace BookApplication.Service.Implementation;

public class BookService : IBookService
{
    private readonly IRepository<Book> _bookRepository;

    public BookService(IRepository<Book> bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public List<Book> GetAllBooks()
    {
        return _bookRepository.GetAll().ToList();
    }

    public Book getDetailsForBook(Guid? id)
    {
        return _bookRepository.Get(id);
    }

    public void CreateNewBook(Book b)
    {
        _bookRepository.Insert(b);
    }

    public void UpdateExistingBook(Book b)
    {
        _bookRepository.Update(b);
    }

    public void DeleteBook(Guid id)
    {
        Book b = _bookRepository.Get(id);
        _bookRepository.Delete(b);
    }
}
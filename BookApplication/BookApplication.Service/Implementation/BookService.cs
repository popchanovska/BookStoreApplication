using System.Text;
using BookApplication.Domain.Domain;
using BookApplication.Repository.Interface;
using BookApplication.Service.Interface;

namespace BookApplication.Service.Implementation;

public class BookService : IBookService
{
    private readonly IRepository<Book> _bookRepository;
    private readonly IRepository<Author> _authorRepository; 
    private readonly IRepository<Publisher> _publisherRepository;

    public BookService(IRepository<Book> bookRepository, IRepository<Author> authorRepository, IRepository<Publisher> publisherRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _publisherRepository = publisherRepository;

    }

    public List<Book> GetAllBooks()
    {
        var books = _bookRepository.GetAll();
        foreach(var book in books)
        {
            book.Publisher= _publisherRepository.Get(book.PublisherId);
            book.Author= _authorRepository.Get(book.AuthorId);
        }
        return books.ToList();
    }

    public Book getDetailsForBook(Guid? id)
    {
        var book = _bookRepository.Get(id);
        book.Author=_authorRepository.Get(book.AuthorId);
        book.Publisher = _publisherRepository.Get(book.PublisherId);

        return book;
    }

    public void CreateNewBook(Book b)
    {
        b.CoverImage = UploadImage.ConvertImageToBase64(b.CoverImage);
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
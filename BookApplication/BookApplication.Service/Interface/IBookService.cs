using BookApplication.Domain.Domain;

namespace BookApplication.Service.Interface;

public interface IBookService
{
    List<Book> GetAllBooks();
    Book getDetailsForBook(Guid? id);
    void CreateNewBook(Book b);
    void UpdateExistingBook(Book b);
    void DeleteBook(Guid id);
}
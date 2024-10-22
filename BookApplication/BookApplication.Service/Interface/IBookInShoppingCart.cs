using BookApplication.Domain.Domain;

namespace BookApplication.Service.Interface
{
    public interface IBookInShoppingCart
    {

        void AddBookToShoppingCart(BookInShoppingCart bsc);
        void DeleteBookFromShoppingCart(BookInShoppingCart bsc);
        void EditBookInShoppingCart(BookInShoppingCart bsc);
        void EmptyCart(Guid id);
        List<BookInShoppingCart> GetAllBooksInShoppingCart(Guid? id);

        BookInShoppingCart GetBookInShoppingCart(Guid id);  

    }
}

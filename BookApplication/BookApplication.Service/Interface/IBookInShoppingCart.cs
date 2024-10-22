using BookApplication.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApplication.Service.Interface
{
    public interface IBookInShoppingCart
    {

        void AddBookToShoppingCart(BookInShoppingCart bsc);
        void DeleteBookFromShoppingCart(BookInShoppingCart bsc);
        void EditBookInShoppingCart(BookInShoppingCart bsc);
        List<BookInShoppingCart> GetAllBooksInShoppingCart(Guid? id);
        void EmptyCart(Guid Id);
        BookInShoppingCart GetBookInShoppingCart(Guid id);  

    }
}

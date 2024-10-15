using BookApplication.Domain.Domain;
using BookApplication.Repository.Interface;
using BookApplication.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace BookApplication.Service.Implementation
{
    public class BookInShoppingCartService : IBookInShoppingCart
    {
        private readonly IRepository<BookInShoppingCart> _bookInShoppingCartRepository;
        private readonly IShoppingCartsService _shoppingCartService;

        public BookInShoppingCartService(IRepository<BookInShoppingCart> bookInShoppingCartRepository, IShoppingCartsService shoppingCartService)
        {
            _bookInShoppingCartRepository = bookInShoppingCartRepository;
            _shoppingCartService = shoppingCartService;
        }

        public void AddBookToShoppingCart(BookInShoppingCart bsc)
        {
            var existingBookInCart = _bookInShoppingCartRepository
                   .GetAll()
                   .FirstOrDefault(x => x.Book?.Id == bsc.Book.Id && x.ShoppingCart.Id == bsc.ShoppingCart.Id);

            if (existingBookInCart==null)
            {
                _bookInShoppingCartRepository.Insert(bsc);
            }
            else
            {
                existingBookInCart.Quantity += bsc.Quantity;
                _bookInShoppingCartRepository.Update(existingBookInCart);
            }
        }

        public void DeleteBookFromShoppingCart(BookInShoppingCart b)
        {
            BookInShoppingCart bookInShoppingCart = GetBookInShoppingCart(b.Id);
            _bookInShoppingCartRepository.Delete(bookInShoppingCart);
        }

        public void EditBookInShoppingCart(BookInShoppingCart bsc)
        {
            BookInShoppingCart bookInShoppingCart = GetBookInShoppingCart(bsc.Id);
            _bookInShoppingCartRepository.Update(bookInShoppingCart);
        }


        public List<BookInShoppingCart> GetAllBooksInShoppingCart(Guid id)
        {
            return _bookInShoppingCartRepository.GetAll().Where(x => x.ShoppingCart.Id == id).ToList();
        }

        public BookInShoppingCart GetBookInShoppingCart(Guid id)
        {
            return _bookInShoppingCartRepository.Get(id);
        }
    }
}

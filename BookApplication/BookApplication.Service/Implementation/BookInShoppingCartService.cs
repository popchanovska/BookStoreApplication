using BookApplication.Domain.Domain;
using BookApplication.Repository.Interface;
using BookApplication.Service.Interface;
namespace BookApplication.Service.Implementation
{
    public class BookInShoppingCartService : IBookInShoppingCart
    {
        private readonly IRepository<BookInShoppingCart> _bookInShoppingCartRepository;
        private readonly IShoppingCartsService _shoppingCartService;
        private readonly IBookService _bookService;

        public BookInShoppingCartService(IRepository<BookInShoppingCart> bookInShoppingCartRepository, IShoppingCartsService shoppingCartService, IBookService bookService)
        {
            _bookInShoppingCartRepository = bookInShoppingCartRepository;
            _shoppingCartService = shoppingCartService;
            _bookService = bookService;
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
            _shoppingCartService.UpdateExistingShoppingCart(bsc.ShoppingCart);
        }

        public void EmptyCart(Guid Id)
        {
            GetAllBooksInShoppingCart(Id).ToList().ForEach(x => _bookInShoppingCartRepository.Delete(x));
        }
        
        public void DeleteBookFromShoppingCart(BookInShoppingCart b)
        {
            BookInShoppingCart bookInShoppingCart = GetBookInShoppingCart(b.Id);
            _bookInShoppingCartRepository.Delete(bookInShoppingCart);
            _shoppingCartService.UpdateExistingShoppingCart(b.ShoppingCart);

        }

        public void EditBookInShoppingCart(BookInShoppingCart bsc)
        {
            BookInShoppingCart bookInShoppingCart = GetBookInShoppingCart(bsc.Id);
            _bookInShoppingCartRepository.Update(bookInShoppingCart);
            _shoppingCartService.UpdateExistingShoppingCart(bsc.ShoppingCart);

        }


        public List<BookInShoppingCart> GetAllBooksInShoppingCart(Guid? id)
        {
            var booksInShoppingCart = _bookInShoppingCartRepository.GetAll().Where(x => x.ShoppingCartId == id).ToList();
            foreach(var book in booksInShoppingCart)
            {
                book.Book = _bookService.getDetailsForBook(book.BookId);
            }
            return booksInShoppingCart;
        }

        public BookInShoppingCart GetBookInShoppingCart(Guid id)
        {
            var book = _bookInShoppingCartRepository.Get(id);
            book.Book = _bookService.getDetailsForBook(book.BookId);
            book.ShoppingCart = _shoppingCartService.GetDetailsForShoppingCart(book.ShoppingCartId);
            return book;
        }
    }
}

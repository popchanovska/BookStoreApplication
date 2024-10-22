using BookApplication.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApplication.Service
{
    public class MainService
    {
        public IAddressService Address { get; set; }
        public IBookInOrderService BookInOrder { get; set; }
        public IBookService Book { get; set; }
        public IOrderService Order { get; set; }
        public IShoppingCartsService ShoppingCart { get; set; }
        public IPublisherService Publisher { get; set; }
        public IAuthorService Author { get; set; }
        public IBookInShoppingCart BookInShoppingCart { get; set; }

        public MainService(IAddressService address, IBookInOrderService bookInOrder, IBookService book, IOrderService order, IShoppingCartsService shoppingCart, IPublisherService publisher, IAuthorService author, IBookInShoppingCart bookInShoppingCart)
        {
            Address = address;
            BookInOrder = bookInOrder;
            Book = book;
            Order = order;
            ShoppingCart = shoppingCart;
            Publisher = publisher;
            Author = author;
            BookInShoppingCart = bookInShoppingCart;
        }
    }
}

using BookApplication.Domain.Domain;
using BookApplication.Repository.Interface;
using BookApplication.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApplication.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartsService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IUserRepository _userRepository;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IUserRepository userRepository) {
            
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository=userRepository;

        }
        public List<ShoppingCart> GetAllShoppingCarts()
        {
            return _shoppingCartRepository.GetAll().ToList();

        }

        public ShoppingCart GetDetailsForShoppingCart(Guid? id)
        {
                return _shoppingCartRepository.Get(id);
        }

        public void CreateNewShoppingCart(ShoppingCart s)
        {
            _shoppingCartRepository.Insert(s);
        }

        public void UpdateExistingShoppingCart(ShoppingCart s)
        {
            _shoppingCartRepository.Update(s);
        }

        public void DeleteShoppingCart(Guid id)
        {
            ShoppingCart shoppingCart = GetDetailsForShoppingCart(id);
            _shoppingCartRepository.Delete(shoppingCart);
        }
        //public void AddBookToShoppingCart(Guid id, Book b)
        //{
        //    ShoppingCart shoppingCart = GetDetailsForShoppingCart(id);
        //    shoppingCart.Books.Add(b);
        //    _shoppingCartRepository.Update(shoppingCart);
        //}   

        public List<ShoppingCart> GetAllShoppingCartsForUser(string userId)
        {
            var user = _userRepository.Get(userId);
            return _shoppingCartRepository.GetAll().Where(x => x.OwnerId.Equals(user.Id)).ToList();

        }
    }
}

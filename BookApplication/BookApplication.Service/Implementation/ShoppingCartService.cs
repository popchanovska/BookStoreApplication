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

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository) {
            
            _shoppingCartRepository = shoppingCartRepository;

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
    }
}

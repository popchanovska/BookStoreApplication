using BookApplication.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace BookApplication.Service.Interface
{
    public interface IShoppingCartsService
    {
        List<ShoppingCart> GetAllShoppingCarts();
        ShoppingCart GetDetailsForShoppingCart(Guid? id);
        void CreateNewShoppingCart(ShoppingCart s);
        void UpdateExistingShoppingCart(ShoppingCart s);
        void DeleteShoppingCart(Guid id);

    }
}

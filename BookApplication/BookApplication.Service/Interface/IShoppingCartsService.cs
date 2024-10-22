using BookApplication.Domain.Domain;

namespace BookApplication.Service.Interface
{
    public interface IShoppingCartsService
    {
        List<ShoppingCart> GetAllShoppingCarts();
        ShoppingCart GetDetailsForShoppingCart(Guid? id);
        void CreateNewShoppingCart(ShoppingCart s);
        void UpdateExistingShoppingCart(ShoppingCart s);
        void DeleteShoppingCart(Guid id);

        List<ShoppingCart> GetAllShoppingCartsForUser(string userId);
    }
}

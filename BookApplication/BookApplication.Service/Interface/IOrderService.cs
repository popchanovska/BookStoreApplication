using BookApplication.Domain.Domain;

namespace BookApplication.Service.Interface;

public interface IOrderService
{
    List<Order> GetAllOrders();
    Order GetDetailsForOrder(BaseEntity id);
    void AddNewOrder(Order o);
    void DeleteOrder(BaseEntity id);
    void UpdateOrder(Order o);
}
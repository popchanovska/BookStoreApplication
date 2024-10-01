using BookApplication.Domain.Domain;

namespace BookApplication.Service.Interface;

public interface IOrderService
{
    List<Order> GetAllOrders();
    Order GetDetailsForOrder(BaseEntity id);
}
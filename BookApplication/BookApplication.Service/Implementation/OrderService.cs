using BookApplication.Domain.Domain;
using BookApplication.Repository.Interface;
using BookApplication.Service.Interface;

namespace BookApplication.Service.Implementation;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public List<Order> GetAllOrders()
    {
        return _orderRepository.GetAllOrders();
    }

    public Order GetDetailsForOrder(BaseEntity id)
    {
        var order = _orderRepository.GetDetailsForOrder(id);
        return order;
    }

    public void AddNewOrder(Order o)
    {
        try
        {
            _orderRepository.Insert(o);
        }
        catch (ArgumentNullException e)
        {
            throw new Exception(e.Message);
        }
    }

    public void DeleteOrder(BaseEntity id)
    {
        try
        {
            _orderRepository.Delete(id);
        }
        catch (ArgumentNullException e)
        {
            throw new Exception(e.Message);
        }
    }

    public void UpdateOrder(Order o)
    {
        _orderRepository.Update(o);
    }
}
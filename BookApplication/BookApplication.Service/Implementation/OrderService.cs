using BookApplication.Domain.Domain;
using BookApplication.Repository.Interface;
using BookApplication.Service.Interface;

namespace BookApplication.Service.Implementation;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IAddressService  _addressService;

    public OrderService(IOrderRepository orderRepository, IAddressService addressService)
    {
        _orderRepository = orderRepository;
        _addressService = addressService;
    }


    public List<Order> GetAllOrders()
    {
        return _orderRepository.GetAllOrders();
    }

    public Order GetDetailsForOrder(BaseEntity id)
    {

        var order = _orderRepository.GetDetailsForOrder(id);
        order.Address=_addressService.GetAddress(order.AddressId);
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
}
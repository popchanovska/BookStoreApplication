using BookApplication.Domain.Domain;
using BookApplication.Repository.Interface;
using BookApplication.Service.Interface;

namespace BookApplication.Service.Implementation;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IAddressService  _addressService;
    private readonly IBookInShoppingCart _bookInShoppingCart;
    private readonly IRepository<Order> _iOrderRepository;

    public OrderService(IOrderRepository orderRepository, IAddressService addressService, IBookInShoppingCart bookInShoppingCart, IRepository<Order> iOrderRepository)
    {
        _orderRepository = orderRepository;
        _addressService = addressService;
        _bookInShoppingCart = bookInShoppingCart;
        _iOrderRepository = iOrderRepository;
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

    public void UpdateOrder(Order o)
    {
        _iOrderRepository.Update(o);
    }
}
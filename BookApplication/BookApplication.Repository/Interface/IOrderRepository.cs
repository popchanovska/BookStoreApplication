using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApplication.Domain.Domain;

namespace BookApplication.Repository.Interface
{
    public interface IOrderRepository
    {
        List<Order> GetAllOrders();
        Order GetDetailsForOrder(BaseEntity id);
        Guid GetShoppingCartIdForOrder(BaseEntity id);
        void Insert(Order o);
        void Delete(BaseEntity id);
        void Update(Order o);
    }
}

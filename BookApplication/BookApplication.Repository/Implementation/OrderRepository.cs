using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApplication.Domain.Domain;
using BookApplication.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookApplication.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Order> entities;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Order>();
        }

        public List<Order> GetAllOrders()
        {
            return entities
                // .Include(z => z.shoppingCart)
                // .Include(z => z.User)
                .ToList();
        }

        public Order GetDetailsForOrder(BaseEntity id)
        {
            return entities
                .Include(z => z.BooksInOrder)
                .ThenInclude(b => b.Book)
                // .Include(z => z.shoppingCart)
                .Include(z => z.User)
                .SingleOrDefaultAsync(z => z.Id == id.Id).Result;
        }

        public void Insert(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("entity");
            }

            entities.Add(order);
            context.SaveChanges();
        }

        public void Delete(BaseEntity id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("entity");
            }

            var entity = entities.SingleOrDefault(s => s.Id == id.Id);
            entities.Remove(entity);
            context.SaveChanges();
        }

        public void Update(Order o)
        {
            if (o == null)
            {
                throw new ArgumentNullException("entity");
            }

            entities.Update(o);
            context.SaveChanges();
        }
    }
}
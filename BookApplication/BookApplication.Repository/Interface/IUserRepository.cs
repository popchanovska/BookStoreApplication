using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApplication.Domain.Identity;

namespace BookApplication.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<BookAppUser> GetAll();
        BookAppUser Get(string? id);
        string GetOnlyUsername(string? id);
        void Insert(BookAppUser entity);
        void Update(BookAppUser entity);
        void Delete(BookAppUser entity);
    }
}

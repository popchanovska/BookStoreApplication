using BookApplication.Domain.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApplication.Service.Interface
{
    public interface IAddressService
    {
        
        string GetAddress(Guid id);
        void CreateAddress(Address a);
        void UpdateAddress(Address a);
        void DeleteAddress(Address a);
        List<Address> GetAllAddresses();
    }
}

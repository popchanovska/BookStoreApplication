using BookApplication.Domain.Domain;

namespace BookApplication.Service.Interface
{
    public interface IAddressService
    {
        
        Address GetAddress(Guid id);
        void CreateAddress(Address a);
        void UpdateAddress(Address a);
        void DeleteAddress(Address a);
        List<Address> GetAllAddresses();
    }
}

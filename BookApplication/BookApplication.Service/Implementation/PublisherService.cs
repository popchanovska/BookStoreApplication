using BookApplication.Domain.Domain;
using BookApplication.Repository.Interface;
using BookApplication.Service.Interface;

namespace BookApplication.Service.Implementation
{
    public class PublisherService : IPublisherService
    {
        private readonly IRepository<Publisher> _publisherRepository;
        private readonly IAddressService _addressService;

        public PublisherService(IRepository<Publisher> publisherRepository, IAddressService addressService)
        {
            _publisherRepository = publisherRepository;
            _addressService = addressService;
        }

        public void CreatePublisher(Publisher publisher)
        {
            _publisherRepository.Insert(publisher);
        }

        public void DeletePublisher(Guid id)
        {
            _publisherRepository.Delete(_publisherRepository.Get(id));
        }

        public List<Publisher> GetAllPublishers()
        {
            return _publisherRepository.GetAll().ToList();

        }

        public Publisher GetPublisher(Guid id)
        {
            var publisher = _publisherRepository.Get(id);
            publisher.Address = _addressService.GetAddress(publisher.AddressId);
            return publisher;
        }

        public void UpdatePublisher(Publisher publisher)
        {
            _publisherRepository.Update(publisher);
        }
    }
}

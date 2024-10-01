using BookApplication.Domain.Domain;
using BookApplication.Repository.Interface;
using BookApplication.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApplication.Service.Implementation
{
    public class PublisherService : IPublisherService
    {
        private readonly IRepository<Publisher> _publisherRepository;

        public PublisherService(IRepository<Publisher> publisherRepository) {
            _publisherRepository = publisherRepository;
        
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
            return _publisherRepository.Get(id);
        }

        public void UpdatePublisher(Publisher publisher)
        {
            _publisherRepository.Update(publisher);
        }
    }
}

using BookApplication.Domain.Domain;


namespace BookApplication.Service.Interface
{
    public interface IPublisherService
    {
        List<Publisher> GetAllPublishers();
        Publisher GetPublisher(Guid id);
        void CreatePublisher(Publisher publisher);
        void UpdatePublisher(Publisher publisher);
        void DeletePublisher(Guid id);

    }
}

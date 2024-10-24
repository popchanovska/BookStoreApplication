
namespace BookShopAdmin.Models
{
    public class Publisher : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid? AddressId { get; set; }
        public Address? Address { get; set; }
        public ICollection<Book>? Books { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
        public string PublisherDisplay => ToString();


    }
}

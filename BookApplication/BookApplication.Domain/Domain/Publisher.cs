using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApplication.Domain.Domain
{
    public class Publisher : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid AddressId { get; set; }
        public Address Address { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}

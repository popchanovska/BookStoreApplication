using BookShopAdmin.Models;


namespace BookShopAdmin.Models
{
    public class Author : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Image { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}

using Microsoft.AspNetCore.Identity;

namespace BookApplication.Domain.Identity;

public class BookAppUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
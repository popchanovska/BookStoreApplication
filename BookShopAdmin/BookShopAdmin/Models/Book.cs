using BookShopAdmin.Models.Enums;

namespace BookShopAdmin.Models;

public class Book : BaseEntity
{
    public Guid AuthorId { get; set; }
    public Author? Author { get; set; }
    public string Title { get; set; }
    public string ISBN { get; set; }
    public double Price { get; set; }
    public string CoverImage { get; set; }
    public int PublicatonYear { get; set; }
    public Boolean IsHardcover { get; set; }
    public double Rating { get; set; }
    public Genre Genre { get; set; }
    public Guid PublisherId { get; set; }
    public Publisher? Publisher { get; set; }
}
using BookApplication.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApplication.Domain.DTOs;

namespace BookApplication.Service.Interface
{
    public interface IAuthorService
    {
        List<Author> GetAllAuthors();
        IEnumerable<AuthorDTO> GetNamesForAuthors();
        Author getDetailsForAuthor(Guid? id);
        void CreateNewAuthor(Author a);
        void UpdateExistingAuthor(Author a);
        void DeleteAuthor(Guid id);
    }
}

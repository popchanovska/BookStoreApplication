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
    public class AuthorService : IAuthorService
    {
        private readonly IRepository<Author> _authorRepository;

        public AuthorService(IRepository<Author> authorRepository) { 
            _authorRepository = authorRepository;

        }
        public void CreateNewAuthor(Author a)
        {
            _authorRepository.Insert(a);
        }

        public void DeleteAuthor(Guid id)
        {
            Author a = _authorRepository.Get(id);
            _authorRepository.Delete(a);
        }

        public List<Author> GetAllAuthors()
        {
            return _authorRepository.GetAll().ToList();

        }

        public Author getDetailsForAuthor(Guid? id)
        {
            return _authorRepository.Get(id) as Author;
        }

        public void UpdateExistingAuthor(Author a)
        {
            _authorRepository.Update(a);
        }
    }
}

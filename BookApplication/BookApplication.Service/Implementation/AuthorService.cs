﻿using BookApplication.Domain.Domain;
using BookApplication.Repository.Interface;
using BookApplication.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApplication.Domain.DTOs;

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
            if (a.Image != null)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(a.Image);
                a.Image = Convert.ToBase64String(bytes);
            }
            
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

        public IEnumerable<AuthorDTO> GetNamesForAuthors()
        {
            var authors = _authorRepository.GetAll()
                .Select(a => new AuthorDTO
                {
                    Id = a.Id,
                    FullName = a.FirstName + " " + a.LastName
                }).ToList();

            return authors;
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
﻿using BookApplication.Domain.Domain;
using BookApplication.Repository.Interface;
using BookApplication.Service.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApplication.Service.Implementation
{

    public class AddressService : IAddressService
    {
        private readonly IRepository<Address> _addressRepository;

        public AddressService(IRepository<Address> addressRepository)
        {
            _addressRepository = addressRepository;
        }
        public void CreateAddress(Address a)
        {
            _addressRepository.Insert(a);
        }

        public void DeleteAddress(Address a)
        {
            _addressRepository.Delete(a);
        }

        public Address GetAddress(Guid id)
        {
            return _addressRepository.Get(id);
        }

        public List<Address> GetAllAddresses()
        {
           
                return _addressRepository.GetAll().ToList();
            
        }

        public void UpdateAddress(Address a)
        {
            _addressRepository.Update(a);
        }
       
    }
}

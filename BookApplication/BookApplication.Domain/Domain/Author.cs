﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BookApplication.Domain.Domain
{
    public class Author : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Image { get; set; }
        public string FullName => $"{FirstName} {LastName}";  // Add this property

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}

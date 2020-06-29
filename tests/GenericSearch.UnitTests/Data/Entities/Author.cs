using System;
using System.Collections.Generic;

namespace GenericSearch.UnitTests.Data.Entities
{
    public class Author
    {
        private Author()
        {
        }

        public Author(int id, string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            DateOfBirth = dateOfBirth;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }

        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
using FakeAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeAPI.Entities
{
    public class User
    {
        public String FirstName { get; private set; }
        public String LastName { get; private set; }
        public String Email { get; private set; }
        public String Password { get; private set; }
        public String Phone { get; private set; }
        public Role Role { get; private set; }
        public Company Company { get; private set; }
        public List<Order> Orders { get; private set; }

        public User(string firstName, string lastName, string email, string password, string phone, Role role, Company company, List<Order> orders)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Phone = phone;
            Role = role;
            Company = company;
            Orders = orders;
        }
        public User() { }
    }
}

using System;
using System.Collections.Generic;
using ChatBot.Models.DTOs;
using ChatBot.Models.Enums;


namespace Fake_API.Entities
{
    public class UserDTOapi
    {
        public Guid id { get;  set; }
        public string firstName { get;  set; }
        public string lastName { get;  set; }
        public string email { get;  set; }
        public string password { get;  set; }
        public string phone { get;  set; }
        public Role Role { get;  set; }
        public Company Company { get;  set; }
        public List<Order> Orders { get;  set; }

        public UserDTOapi(Guid id, string firstName, string lastName, string email, string password, string phone, Role role, Company company, List<Order> orders)
        {
            id = id;
            firstName = firstName;
            lastName = lastName;
            email = email;
            password = password;
            phone = phone;
            Role = role;
            Company = company;
            Orders = orders;
        }
        public UserDTOapi() { }
    }
}

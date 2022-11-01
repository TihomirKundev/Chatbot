using FakeAPI.Enums;

namespace FakeAPI.Entities
{
    public class User
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Phone { get; private set; }
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

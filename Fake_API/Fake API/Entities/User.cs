using Fake_API.Enums;

namespace Fake_API.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Phone { get; private set; }
        public Role Role { get; private set; }
        public Company Company { get; private set; }
        public List<Order> Orders { get; private set; }

        public User(Guid id, string firstName, string lastName, string email, string password, string phone, Role role, Company company, List<Order> orders)
        {
            Id = id;
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

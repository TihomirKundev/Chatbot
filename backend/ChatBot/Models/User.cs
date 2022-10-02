using System;
using System.Text.RegularExpressions;

namespace ChatBot.Models;

public class User : Anonymous
{
    public User(string firstName,
                string lastName,
                string email,
                string phone,
                Role role)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Role = role;
    }
    public User(Guid id,
                string firstName,
                string lastName,
                string email,
                string phone,
                Role role) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Role = role;
    }

    public string FirstName
    {
        get => _firstName;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(FirstName));
            if (!_nameRegex.IsMatch(value))
                throw new FormatException("First name should only contain alphabetical characters.");
            _firstName = value;
        }
    }

    public string LastName
    {
        get => _lastName;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(LastName));
            if (!_nameRegex.IsMatch(value))
                throw new FormatException("Last name should only contain alphabetical characters.");
            _lastName = value;
        }
    }

    public string Email
    {
        get => _email;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(Email));
            //if (!_emailRegex.IsMatch(value))
            //    throw new FormatException("Email is not valid.");
            _email = value;
        }
    }

    public string Phone
    {
        get => _phone;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(Phone));
            //if (!_phoneRegex.IsMatch(value))
            //    throw new FormatException("Phone number is not valid.");
            _phone = value;
        }
    }

    public Role Role { get; }

    private string _firstName = null!;

    private string _lastName = null!;

    private string _email = null!;

    private string _phone = null!;

    private static readonly Regex _nameRegex = new("^[a-zA-Z]*$");

    //private static readonly Regex _emailRegex = new ("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$");

    //private static readonly Regex _phoneRegex = new ("^\\+?\\d{1,4}?[-.\\s]?\\(?\\d{1,3}?\\)?[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,9}$");
}

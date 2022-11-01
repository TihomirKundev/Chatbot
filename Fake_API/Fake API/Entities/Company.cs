using FakeAPI.Enums;

namespace FakeAPI.Entities
{
    public class Company
    {

        public string Name { get; private set; }
        public string WebsiteUrl { get; private set; }
        public string Address { get; private set; }
        public string ZipCode { get; private set; }
        public string City { get; private set; }
        public Countries Country { get; private set; }
        public LegalForms LegalForm { get; private set; }

        public Company(string name, string websiteUrl, string address, string zipCode, string city, Countries country, LegalForms legalForm)
        {
            Name = name;
            WebsiteUrl = websiteUrl;
            Address = address;
            ZipCode = zipCode;
            City = city;
            Country = country;
            LegalForm = legalForm;
        }

        public Company()
        {
        }
    }
}

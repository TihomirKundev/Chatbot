using FakeAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeAPI.Entities
{
    public class Company
    {

        public String Name { get; private set; }
        public String WebsiteUrl { get; private set; }
        public string Address { get; private set; }
        public String ZipCode { get; private set; }
        public String City { get; private set; }
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

using FakeAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeAPI.Entities
{
    public class Vehicle
    {
        public int ReferenceNum { get; private set; }
        public String Name { get; private set; }
        public Boolean Availability { get; private set; }
        public String Location { get; private set; }
        public Double Price { get; private set; }
        public String City { get; private set; }
        public Countries Country { get; private set; }

        public Vehicle(int referenceNum, string name, bool availability, string location, double price, string city, Countries country)
        {
            ReferenceNum = referenceNum;
            Name = name;
            Availability = availability;
            Location = location;
            Price = price;
            City = city;
            Country = country;
        }
    }
}

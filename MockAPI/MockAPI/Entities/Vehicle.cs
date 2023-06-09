﻿using MockAPI.Enums;

namespace MockAPI.Entities
{
    public class Vehicle
    {
        public int ReferenceNum { get; private set; }
        public string Name { get; private set; }
        public bool Availability { get; private set; }
        public string Location { get; private set; }
        public double Price { get; private set; }
        public string City { get; private set; }
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

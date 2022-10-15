using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeAPI.Entities
{
    public class Order
    {
        public List<Vehicle> Vehicles { get; private set; }
        public Boolean Status { get; private set; }

        public Order(List<Vehicle> vehicles, bool status)
        {
            Vehicles = vehicles;
            Status = status;
        }
    }
}

namespace MockAPI.Entities
{
    public class Order
    {
        public List<Vehicle> Vehicles { get; private set; }
        public bool Status { get; private set; }

        public Order(List<Vehicle> vehicles, bool status)
        {
            Vehicles = vehicles;
            Status = status;
        }
    }
}

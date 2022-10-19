using FakeAPI.Entities;
using FakeAPI.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FakeAPI.DAL
{
    public class DALUser
    {
        public User CheckAndGetUSer(string email, string password)
        {
            User user = new User();
            SqlConnection conn = new SqlConnection("Server=tcp:group1.database.windows.net,1433;Initial Catalog=Group;Persist Security Info=False;User ID=BasWorldShit;Password=BasWorld1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM [user] WHERE email = '" + email + "' AND [password] = '" + password + "'", conn);
            string res = (string)command.ExecuteScalar();
            if (String.IsNullOrEmpty(res))
            {
                return null;
            }
            else
            {
                return GetUser(email);
            }
        }
        private User GetUser(string email)
        {
            User user = new User();
            SqlConnection conn = new SqlConnection("Server=tcp:group1.database.windows.net,1433;Initial Catalog=Group;Persist Security Info=False;User ID=BasWorldShit;Password=BasWorld1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM [user] WHERE email = '" + email + "'", conn);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            foreach(DataRow dr in dataTable.Rows)
            {
                String firstName = Convert.ToString(dr["first_name"]);
                String lastName = Convert.ToString(dr["last_name"]);
                String password = Convert.ToString(dr["password"]);
                String phone = Convert.ToString(dr["phone"]);
                Role role = Role.Customer;
                Company company = GetCompany(Convert.ToInt32(dr["company_id"]));
                List<Order> orders = GetOrdersForUser(email);
                user = new User(firstName, lastName, email, password, phone, role, company, orders);
            }
            conn.Close();
            return user;
        }
        private Company GetCompany(int companyId)
        {
            Company company = new Company();
            SqlConnection conn = new SqlConnection("Server=tcp:group1.database.windows.net,1433;Initial Catalog=Group;Persist Security Info=False;User ID=BasWorldShit;Password=BasWorld1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM company WHERE id = " + companyId + "", conn);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            foreach(DataRow dr in dataTable.Rows)
            {
                String name = Convert.ToString(dr["name"]);
                String websiteUrl = Convert.ToString(dr["website_url"]);
                String address = Convert.ToString(dr["address"]);
                String zipCode = Convert.ToString(dr["zip_code"]);
                String city = Convert.ToString(dr["city"]);
                Countries country = (Countries)Enum.Parse(typeof(Countries), Convert.ToString(dr["country_code"]));
                LegalForms legalForm = (LegalForms)Enum.Parse(typeof(LegalForms), Convert.ToString(dr["legal_form"]));
                company = new Company(name, websiteUrl, address, zipCode, city, country, legalForm);
            }
            conn.Close();
            return company;
        }
        private List<Vehicle> GetVehiclesForOrder(int orderNum)
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            SqlConnection conn = new SqlConnection("Server=tcp:group1.database.windows.net,1433;Initial Catalog=Group;Persist Security Info=False;User ID=BasWorldShit;Password=BasWorld1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM vehicle INNER JOIN order_item ON order_item.reference_num = vehicle.reference_num INNER JOIN[order] ON[order].order_num = order_item.order_num WHERE[order].order_num = " + orderNum + "", conn);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            foreach (DataRow dr in dataTable.Rows)
            {
                int referenceNum = Convert.ToInt32(dr["reference_num"]);
                String name = Convert.ToString(dr["name"]);
                bool availability = Convert.ToBoolean(dr["availability"]);
                String location = Convert.ToString(dr["location"]);
                Double price = Convert.ToDouble(dr["price"]);
                Countries country = (Countries)Enum.Parse(typeof(Countries), Convert.ToString(dr["country_code"]));
                String city = Convert.ToString(dr["city"]);
                vehicles.Add(new Vehicle(referenceNum, name, availability, location, price, city, country));
            }
            return vehicles;
        }
        private List<Order> GetOrdersForUser(string email)
        {
            List<Order> orders = new List<Order>();
            SqlConnection conn = new SqlConnection("Server=tcp:group1.database.windows.net,1433;Initial Catalog=Group;Persist Security Info=False;User ID=BasWorldShit;Password=BasWorld1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM [order] INNER JOIN client_order ON client_order.order_id = [order].order_num WHERE email = '" + email + "'", conn);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            foreach (DataRow dr in dataTable.Rows)
            {
                orders.Add(new Order(GetVehiclesForOrder(Convert.ToInt32(dr["order_num"])), Convert.ToBoolean(dr["status"])));
            }
            return orders;
        }
    }
}

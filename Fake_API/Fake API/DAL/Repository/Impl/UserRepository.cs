using System.Data;
using System.Data.SqlClient;
using System.Text;
using Fake_API.Entities;
using Fake_API.Enums;

namespace Fake_API.DAL.Repository.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly string connString =
            "connection string goes here";

        public User CheckAndGetUSer(string email, string password)
        {
            User user = new User();
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM [user] WHERE email = @email AND password = @password", conn);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@password", password);
            string res = (string)command.ExecuteScalar();
            if (string.IsNullOrEmpty(res))
            {
                return null;
            }
            else
            {
                return GetByEmail(email)!;
            }
        }

        public User? GetByEmail(string email)
        {
            string query =
                "SELECT id, email, first_name, last_name, password, phone, company_id, role " +
                "FROM [user] " +
                "WHERE email = @Email";

            using var rdr = SqlHelper.ExecuteReader(
                connString,
                query,
                new SqlParameter("@Email", email));

            while (rdr.Read())
            {
                Company company = GetCompany(rdr.GetInt32("company_id"));
                List<Order> orders = GetOrdersForUser(rdr.GetString("id"));

                return new User(
                    Guid.Parse(rdr.GetString("id")),
                    rdr.GetString("first_name"),
                    rdr.GetString("last_name"),
                    rdr.GetString("email"),
                    rdr.GetString("password"),
                    rdr.GetString("phone"),
                    (Role) rdr.GetInt32("role"), //fix?
                    company,
                    orders
                    );
            }
            return null;
        }

        public User? GetById(Guid id)
        {
            var query =
                "SELECT id, email, first_name, last_name, password, phone, company_id, role " +
                "FROM [user] " +
                "WHERE id = @Id";

            using var rdr = SqlHelper.ExecuteReader(
                connString,
                query,
                new SqlParameter("@Id", id));

            if (!rdr.Read())
                return null;

            Company company = GetCompany(rdr.GetInt32("company_id"));
            List<Order> orders = GetOrdersForUser(rdr.GetString("id"));

            return new User(
                Guid.Parse(rdr.GetString("id")),
                rdr.GetString("first_name"),
                rdr.GetString("last_name"),
                rdr.GetString("email"),
                rdr.GetString("password"),
                rdr.GetString("phone"),
                (Role) rdr.GetInt32("role"), //fix?
                company, orders);

        }

        public List<User> GetAll()
        {
            List<User> userList = new List<User>();

            var query = "SELECT id, email, first_name, last_name, password, phone, company_id, role FROM [user]";

            using var rdr = SqlHelper.ExecuteReader(
                connString, query);

            if (!rdr.HasRows)
                return userList;

            while (rdr.Read())
            {
                Company company = GetCompany(rdr.GetInt32("company_id"));
                List<Order> orders = GetOrdersForUser(rdr.GetString("id"));

                User user = new User(
                    Guid.Parse(rdr.GetString("id")),
                    rdr.GetString("first_name"),
                    rdr.GetString("last_name"),
                    rdr.GetString("email"),
                    rdr.GetString("password"),
                    rdr.GetString("phone"),
                    (Role) rdr.GetInt32("role"), //fix?
                    company,
                    orders
                );
                userList.Add(user);
            }
            return userList;
        }

        private Company GetCompany(int companyId)
        {
            Company company = new Company();
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM company WHERE id = " + companyId + "", conn);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            foreach (DataRow dr in dataTable.Rows)
            {
                string name = Convert.ToString(dr["name"]);
                string websiteUrl = Convert.ToString(dr["website_url"]);
                string address = Convert.ToString(dr["address"]);
                string zipCode = Convert.ToString(dr["zip_code"]);
                string city = Convert.ToString(dr["city"]);
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
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM vehicle INNER JOIN order_item ON order_item.reference_num = vehicle.reference_num INNER JOIN[order] ON[order].order_num = order_item.order_num WHERE[order].order_num = " + orderNum + "", conn);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            foreach (DataRow dr in dataTable.Rows)
            {
                int referenceNum = Convert.ToInt32(dr["reference_num"]);
                string name = Convert.ToString(dr["name"]);
                bool availability = Convert.ToBoolean(dr["availability"]);
                string location = Convert.ToString(dr["location"]);
                double price = Convert.ToDouble(dr["price"]);
                Countries country = (Countries)Enum.Parse(typeof(Countries), Convert.ToString(dr["country_code"]));
                string city = Convert.ToString(dr["city"]);
                vehicles.Add(new Vehicle(referenceNum, name, availability, location, price, city, country));
            }
            return vehicles;
        }
        private List<Order> GetOrdersForUser(string userGuid)
        {
            List<Order> orders = new List<Order>();
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM [order] INNER JOIN client_order ON client_order.order_id = [order].order_num WHERE user_id = @userId", conn);
            command.Parameters.AddWithValue("@userId", userGuid);
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

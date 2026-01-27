using inventory.Models;
using Microsoft.Data.SqlClient;

namespace inventory.Repository
{
    public class RCustomer
    {
        private readonly string connectionString;
        public RCustomer(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //get all customers
        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM customer";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customers.Add(new Customer
                    {
                        Cid = (int)reader["cid"],
                        Cname = reader["cname"].ToString()!,
                        Cphone = reader["cphone"].ToString()!,
                        Caddress = reader["caddress"].ToString(),
                        Cemail = reader["cemail"].ToString(),
                        createdat = (DateTime)reader["createdat"],
                        createdby = reader["createdby"].ToString()!,

                        modifiedat = reader["modifiedat"] == DBNull.Value ? null : (DateTime?)reader["modifiedat"],
                        modifiedby = reader["modifiedby"] == DBNull.Value ? null : reader["modifiedby"].ToString()

                        //modifiedat = (DateTime)reader["modifiedat"],
                        //modifiedby = reader["modifiedby"].ToString()

                    });
                }
            }
            return customers;
        }

        //get customer by id
        public Customer GetCustomerById(int id)
        {
            Customer? customer = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM customer WHERE cid = @cid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Cid",id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    customer = new Customer
                    {
                        Cid = (int)reader["cid"],
                        Cname = reader["cname"].ToString()!,
                        Cphone = reader["cphone"].ToString()!,
                        Caddress = reader["caddress"] == DBNull.Value ? null : reader["caddress"].ToString(),
                        Cemail = reader["cemail"] == DBNull.Value ? null : reader["cemail"].ToString(),
                        createdat =(DateTime) reader["createdat"],
                        createdby = reader["createdby"].ToString()!,
                        modifiedat = reader["modifiedat"] == DBNull.Value ? null : (DateTime?)reader["modifiedat"],
                        modifiedby = reader["modifiedby"] == DBNull.Value ? null : reader["modifiedby"].ToString()

                    };
                }

            }
            return customer;
        }

        //add new customer
        public void AddCustomer(Customer customer)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO customer(cid,cname,cphone,caddress,cemail,createdat,createdby)" +
                                "VALUES (@Cid,@Cname,@Cphone,@Caddress,@Cemail,@createdat,@createdby)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Cid", customer.Cid);
                cmd.Parameters.AddWithValue("@Cname", customer.Cname);
                cmd.Parameters.AddWithValue("@Cphone", customer.Cphone);
                cmd.Parameters.AddWithValue("@Caddress", (object?)customer.Caddress ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Cemail", (object?)customer.Cemail ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@createdat", customer.createdat);
                cmd.Parameters.AddWithValue("@createdby", customer.createdby);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

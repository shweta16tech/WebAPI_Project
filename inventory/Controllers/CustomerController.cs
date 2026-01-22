using inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace inventory.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly string connectionString;

        public CustomerController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //GET: api/User
        [HttpGet]
        public IActionResult GetAllCustomers()
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
                        Cemail = reader["cemail"].ToString()

                    });
                }
            }
            return Ok(customers);
        }

        //GET: api/Customer  get customer by id
        [HttpGet("{id}")]
        public IActionResult GetCustomerById(int id)
        {
            Customer? customer = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM customer WHERE cid = @cid";
                SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Cid", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    customer = new Customer
                    {
                        Cid = (int)reader["cid"],
                        Cname = reader["cname"].ToString()!,
                        Cphone = reader["cphone"].ToString()!,
                        Caddress = reader["caddress"] == DBNull.Value? null : reader["caddress"].ToString(),
                        Cemail = reader["cemail"] == DBNull.Value?null: reader["cemail"].ToString()
                        
                    };
                }

            }
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }


        //POST:api customer

        [HttpPost]
        public IActionResult AddCustomer(Customer customer)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO customer(cid,cname,cphone,caddress,cemail)" +
                                "VALUES (@Cid,@Cname,@Cphone,@Caddress,@Cemail)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Cid", customer.Cid);
                cmd.Parameters.AddWithValue("@Cname", customer.Cname);
                cmd.Parameters.AddWithValue("@Cphone", customer.Cphone);
                cmd.Parameters.AddWithValue("@Caddress", (object?)customer.Caddress??DBNull.Value);
                cmd.Parameters.AddWithValue("@Cemail", (object?)customer.Cemail??DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            return Ok("User added successfully");
        }


        //PUT

    }
}



//public IActionResult Index()
//{
//    return View();
//}
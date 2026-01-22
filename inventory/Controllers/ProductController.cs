using inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly string connectionString;

        public ProductController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //GET: api/Product
        [HttpGet]

        public IActionResult GetAllProducts()
        {
            List<Product> products = new List<Product>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM product";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        Pid = (int)reader["pid"],
                        Pname = reader["pname"].ToString()!,
                        Price = (decimal)reader["price"],
                        Descrip = reader["descrip"] == DBNull.Value ? null : reader["descrip"].ToString(),
                        Pquantity = (int)reader["pquantity"]

                    });
                }
            }
            return Ok(products);
        }


        //Get product by ID

        [HttpGet("{id}")]
        public IActionResult GetProductByID(int id)
        {
            Product? product = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM product WHERE pid=@Pid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Pid", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    product = new Product
                        {
                        Pid = (int)reader["pid"],
                        Pname = reader["pname"].ToString()!,
                        Price = (decimal)reader["price"],
                        Descrip = reader["descrip"] == DBNull.Value? null : reader["descrip"].ToString(),
                        Pquantity = (int)reader["pquantity"]
                    };
                }
            }
            if (product == null)
                return NotFound();
            return Ok(product);
        }



        //Post product

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO product(pid,pname,price,descrip,pquantity)" +
                    "VALUES(@Pid,@Pname,@Price,@Descrip,@Pquantity)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Pid", product.Pid);
                cmd.Parameters.AddWithValue("@Pname", product.Pname);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Descrip", (object?)product.Descrip??DBNull.Value);
                cmd.Parameters.AddWithValue("@Pquantity", product.Pquantity);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            return Ok("Product added successfully");
        } 
    }
}


//public IActionResult Index()
//{
//    return View();
//}
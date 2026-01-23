using inventory.Models;
using Microsoft.Data.SqlClient;
namespace inventory.Repository
{
    public class RProduct
    {
        private readonly string connectionString;

        public RProduct(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //get all product
        public List<Product> GetAllProducts()
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
            return products;
        }

        //get product by id
        public Product GetProductByID(int id)
        {
            Product? product = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM product WHERE pid=@Pid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Pid", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    product = new Product
                    {
                        Pid = (int)reader["pid"],
                        Pname = reader["pname"].ToString()!,
                        Price = (decimal)reader["price"],
                        Descrip = reader["descrip"] == DBNull.Value ? null : reader["descrip"].ToString(),
                        Pquantity = (int)reader["pquantity"]
                    };
                }
            }
            return product;
        }

        //add new product

        public void AddProduct(Product product)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO product(pid,pname,price,descrip,pquantity)" +
                    "VALUES(@Pid,@Pname,@Price,@Descrip,@Pquantity)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Pid", product.Pid);
                cmd.Parameters.AddWithValue("@Pname", product.Pname);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Descrip", (object?)product.Descrip ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Pquantity", product.Pquantity);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

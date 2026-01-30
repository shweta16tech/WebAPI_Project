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
                        Pquantity = (int)reader["pquantity"],
                        createdat = (DateTime)reader["createdat"],
                        createdby = reader["createdby"].ToString()!,
                        modifiedat = reader["modifiedat"] == DBNull.Value ? null : (DateTime?)reader["modifiedat"],
                        modifiedby = reader["modifiedby"] == DBNull.Value ? null : reader["modifiedby"].ToString()

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
                        Pquantity = (int)reader["pquantity"],
                        createdat = (DateTime)reader["createdat"],
                        createdby = reader["createdby"].ToString()!,
                        modifiedat = reader["modifiedat"] == DBNull.Value ? null : (DateTime?)reader["modifiedat"],
                        modifiedby = reader["modifiedby"] == DBNull.Value ? null : reader["modifiedby"].ToString()

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
                cmd.Parameters.AddWithValue("@createdat", product.createdat);
                cmd.Parameters.AddWithValue("@createdby", product.createdby);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }




        // Update product by id
        public void UpdateProduct(Product product)
        {
            //fetch current data
            var existing = GetProductByID(product.Pid);
            if (existing == null) 
                return;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE product 
                         SET pname = @Pname, 
                             price = @Price, 
                             descrip = @Descrip, 
                             pquantity = @Pquantity, 
                             modifiedat = @modifiedat, 
                             modifiedby = @modifiedby 
                         WHERE pid = @Pid";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Pid", product.Pid);

                cmd.Parameters.AddWithValue("@Pname", string.IsNullOrWhiteSpace(product.Pname) || product.Pname == "string"
                    ? existing.Pname : product.Pname);

                cmd.Parameters.AddWithValue("@Price", product.Price <= 0 ? existing.Price : product.Price);

                cmd.Parameters.AddWithValue("@Descrip", product.Descrip == null || product.Descrip == "string"
                    ? (object?)existing.Descrip ?? DBNull.Value : product.Descrip);

                cmd.Parameters.AddWithValue("@Pquantity", product.Pquantity <= 0 ? existing.Pquantity : product.Pquantity);

                cmd.Parameters.AddWithValue("@modifiedat", DateTime.Now);
                cmd.Parameters.AddWithValue("@modifiedby", product.modifiedby ?? "System");

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Delete product by id
        public bool DeleteProduct(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM product WHERE pid = @Pid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Pid", id);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }





        //edit product by id
        //delete product by id
    }
}

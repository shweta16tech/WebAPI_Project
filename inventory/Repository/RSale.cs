using inventory.Models;
using Microsoft.Data.SqlClient;

namespace inventory.Repository
{
    public class RSale
    {
        private readonly string connectionString;
        public RSale(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //get all sales
        public List<Sale> GetAllSales()
        {
            List<Sale> sales = new List<Sale>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM sales";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    sales.Add(new Sale
                    {
                        Sid = (int)reader["sid"],
                        Sdate = (DateTime)reader["sdate"],
                        Squantity = (int)reader["squantity"],
                        Pid = (int)reader["pid"],
                        Cid = (int)reader["cid"],
                        Sprice = (decimal)reader["sprice"],
                        Srate = (decimal)reader["srate"],
                        Totalmnt = (decimal)reader["totalmnt"]
                    });
                }
            }
            return sales;
        }

        //get sale by id
        public Sale GetSaleById(int id)
        {
            Sale sale = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM sales WHERE  sid = @Sid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Sid", id);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    sale = new Sale
                    {
                        Sid = (int)reader["sid"],
                        Sdate = (DateTime)reader["sdate"],
                        Squantity = (int)reader["squantity"],
                        Pid = (int)reader["pid"],
                        Cid = (int)reader["cid"],
                        Sprice = (decimal)reader["sprice"],
                        Srate = (decimal)reader["srate"],
                        Totalmnt = (decimal)reader["totalmnt"]
                    };
                }
            }
            return sale;
        }


        //add new sale
        public void AddSale(Sale sale)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO sales(sid,sdate,squantity,pid,cid,sprice,srate,totalmnt)" +
                                "VALUES (@Sid,@Sdate,@Squantity,@Pid,@Cid,@Sprice,@Srate,@Totalmnt)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Sid", sale.Sid);
                cmd.Parameters.AddWithValue("@Sdate", sale.Sdate);
                cmd.Parameters.AddWithValue("@Squantity", sale.Squantity);
                cmd.Parameters.AddWithValue("@Pid", sale.Pid);
                cmd.Parameters.AddWithValue("@Cid", sale.Cid);
                cmd.Parameters.AddWithValue("@Sprice", sale.Sprice);
                cmd.Parameters.AddWithValue("@Srate", sale.Srate);
                cmd.Parameters.AddWithValue("@Totalmnt", sale.Totalmnt);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

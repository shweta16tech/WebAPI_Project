using inventory.Models;
using Microsoft.Data.SqlClient;

namespace inventory.Repository
{
    public class RSaleDetail
    {
        private readonly string connectionString;
        public RSaleDetail(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //Get all SaleDetails
        public List<SaleDetail> GetSaleDetail (int sid,SqlConnection con)
        {
            List<SaleDetail> details = new List<SaleDetail>();

            string query = "SELECT * FROM saledetails WHERE sid = @sid";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@sid", sid);

            using(SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    details.Add(new SaleDetail
                    {
                        SaleDetailId = (int)rdr["saledetailid"],
                        Sid = (int)rdr["sid"],
                        Pid = (int)rdr["pid"],
                        Quantity = (int)rdr["quantity"],
                        Rate = (decimal)rdr["rate"],
                        Price = (decimal)rdr["price"],
                        Total = (decimal)rdr["total"]
                    });
                }
            }
            return details;
        }
    }
}
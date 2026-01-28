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
                        Totalmnt = (decimal)reader["totalmnt"],
                        invoicedate = reader["invoicedate"] == DBNull.Value ? null : (DateTime?)reader["invoicedate"],
                        createdat = (DateTime)reader["createdat"],
                        createdby = reader["createdby"].ToString()!,
                        modifiedat = reader["modifiedat"] == DBNull.Value ? null : (DateTime?)reader["modifiedat"],
                        modifiedby = reader["modifiedby"] == DBNull.Value ? null : reader["modifiedby"].ToString()
                    });
                }
            }
            return sales;
        }

        //get sales with pagination+ filter
        public List<Sale> GetSales(
            int pageNumber,
            int pageSize,
            DateTime? fromDate,
            DateTime? toDate,
            int? pid,
            int? cid)
        {
            List<Sale> sales = new List<Sale>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @" select * from sales
                                where
                                (@FromDate IS NULL OR sdate >= @FromDate)
                                AND (@ToDate IS NULL OR sdate <=@ToDate)
                                AND (@Pid IS NULL OR pid = @Pid)
                                AND (@Cid IS NULL OR cid = @cid)
                                ORDER BY sid
                                OFFSET @Offset ROWS
                                FETCH NEXT @PageSize ROWS ONLY";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@FromDate", (object?)fromDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ToDate", (object?)toDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Pid", (object?)pid ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Cid", (object?)cid ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

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
                        Totalmnt = (decimal)reader["totalmnt"],
                        invoicedate = reader["invoicedate"] == DBNull.Value ? null: (DateTime?)reader["invoicedate"],
                        createdat = (DateTime)reader["createdat"],
                        createdby = reader["createdby"].ToString()!,
                        modifiedat = reader["modifiedat"] == DBNull.Value ? null : (DateTime?)reader["modifiedat"],
                        modifiedby = reader["modifiedby"] == DBNull.Value ? null : reader["modifiedby"].ToString()
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
                        Totalmnt = (decimal)reader["totalmnt"],
                        invoicedate = (DateTime)reader["invoicedate"],
                        createdat = (DateTime)reader["createdat"],
                        createdby = reader["createdby"].ToString()!,
                        modifiedat = reader["modifiedat"] == DBNull.Value ? null : (DateTime?)reader["modifiedat"],
                        modifiedby = reader["modifiedby"] == DBNull.Value ? null : reader["modifiedby"].ToString()

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
                string query = @"INSERT INTO sales(sid,sdate,squantity,pid,cid,sprice,srate,totalmnt,invoicedate,createdat,createdby)" +
                                "VALUES (@Sid,@Sdate,@Squantity,@Pid,@Cid,@Sprice,@Srate,@Totalmnt,@invoicedate,@createdat,@createdby)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Sid", sale.Sid);
                cmd.Parameters.AddWithValue("@Sdate", sale.Sdate);
                cmd.Parameters.AddWithValue("@Squantity", sale.Squantity);
                cmd.Parameters.AddWithValue("@Pid", sale.Pid);
                cmd.Parameters.AddWithValue("@Cid", sale.Cid);
                cmd.Parameters.AddWithValue("@Sprice", sale.Sprice);
                cmd.Parameters.AddWithValue("@Srate", sale.Srate);
                cmd.Parameters.AddWithValue("@Totalmnt", sale.Totalmnt);
                cmd.Parameters.AddWithValue("@invoicedate", sale.invoicedate);
                cmd.Parameters.AddWithValue("@createdat", sale.createdat);
                cmd.Parameters.AddWithValue("@createdby", sale.createdby);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

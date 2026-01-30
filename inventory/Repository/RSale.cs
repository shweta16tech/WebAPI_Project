using inventory.Models;
using Microsoft.Data.SqlClient;

namespace inventory.Repository
{
    public class RSale
    {
        private readonly string connectionString;
        private readonly RSaleDetail _saleDetailRepo;

        public RSale(IConfiguration configuration, RSaleDetail saleDetailRepo)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            _saleDetailRepo = saleDetailRepo;
        }

        //get all sales
        //public List<Sale> GetAllSales()
        //{
        //    List<Sale> sales = new List<Sale>();
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        string query = "SELECT * FROM sales";
        //        SqlCommand cmd = new SqlCommand(query, con);
        //        con.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            sales.Add(new Sale
        //            {
        //                Sid = (int)reader["sid"],
        //                Sdate = (DateTime)reader["sdate"],
        //                //Squantity = (int)reader["squantity"],
        //                //Pid = (int)reader["pid"],
        //                Cid = (int)reader["cid"],
        //                //Sprice = (decimal)reader["sprice"],
        //                //Srate = (decimal)reader["srate"],
        //                Totalmnt = (decimal)reader["totalmnt"],
        //                invoicedate = reader["invoicedate"] == DBNull.Value ? null : (DateTime?)reader["invoicedate"],
        //                createdat = (DateTime)reader["createdat"],
        //                createdby = reader["createdby"].ToString()!,
        //                modifiedat = reader["modifiedat"] == DBNull.Value ? null : (DateTime?)reader["modifiedat"],
        //                modifiedby = reader["modifiedby"] == DBNull.Value ? null : reader["modifiedby"].ToString()
        //            });
        //        }
        //    }
        //    return sales;
        //}


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
                con.Open();
                string query = @" select * from sales
                                where
                                (@FromDate IS NULL OR sdate >= @FromDate)
                                AND (@ToDate IS NULL OR sdate <=@ToDate)
                              
                                AND (@Cid IS NULL OR cid = @cid)
                                ORDER BY sid
                                OFFSET @Offset ROWS
                                FETCH NEXT @PageSize ROWS ONLY";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@FromDate", (object?)fromDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ToDate", (object?)toDate ?? DBNull.Value);
                    //cmd.Parameters.AddWithValue("@Pid", (object?)pid ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Cid", (object?)cid ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            sales.Add(new Sale
                            {
                                Sid = (int)reader["sid"],
                                Sdate = (DateTime)reader["sdate"],
                                Cid = (int)reader["cid"],
                                Totalmnt = (decimal)reader["totalmnt"],
                                invoicedate = reader["invoicedate"] == DBNull.Value ? null : (DateTime?)reader["invoicedate"],
                                createdat = (DateTime)reader["createdat"],
                                createdby = reader["createdby"].ToString()!,
                                modifiedat = reader["modifiedat"] == DBNull.Value ? null : (DateTime?)reader["modifiedat"],
                                modifiedby = reader["modifiedby"] == DBNull.Value ? null : reader["modifiedby"].ToString(),
                                SaleDetails= _saleDetailRepo.GetSaleDetail((int)reader["sid"], con)
                            });
                        }
                    }
                    
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

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        sale = new Sale
                        {
                            Sid = (int)reader["sid"],
                            Sdate = (DateTime)reader["sdate"],
                            Cid = (int)reader["cid"],
                            Totalmnt = (decimal)reader["totalmnt"],
                            invoicedate = (DateTime)reader["invoicedate"],
                            createdat = (DateTime)reader["createdat"],
                            createdby = reader["createdby"].ToString()!,
                            modifiedat = reader["modifiedat"] == DBNull.Value ? null : (DateTime?)reader["modifiedat"],
                            modifiedby = reader["modifiedby"] == DBNull.Value ? null : reader["modifiedby"].ToString()

                        };
                    }
                }
                if (sale != null)
                {
                    sale.SaleDetails = _saleDetailRepo.GetSaleDetail(sale.Sid, con);
                }
            }
            
            return sale;
        }


        // create invoice + details

        public string createinvoice (Sale sale)
        {
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    string getMaxSidQuery = "SELECT ISNULL(MAX(Sid), 0) + 1 FROM sales";
                    SqlCommand getMaxSidCmd = new SqlCommand(getMaxSidQuery, con, tran);
                    int newSid = (int)getMaxSidCmd.ExecuteScalar();

                    // insert into sale(i.e salemaster)
                    string q1 = @"insert into sales (sid,sdate,cid,totalmnt,invoicedate,createdat,createdby)
                                 VALUES (@Sid,@Sdate,@Cid,@Totalmnt,@invoicedate,@createdat,@createdby)";                  
                        SqlCommand c1 = new SqlCommand(q1, con, tran);


                    c1.Parameters.AddWithValue("@Sid", newSid);
                    c1.Parameters.AddWithValue("@Sdate", sale.Sdate);
                    c1.Parameters.AddWithValue("@Cid", sale.Cid);
                    c1.Parameters.AddWithValue("@Totalmnt", sale.Totalmnt);
                    c1.Parameters.AddWithValue("@invoicedate", (object?)sale.invoicedate ?? DBNull.Value);
                    c1.Parameters.AddWithValue("@createdat", sale.createdat);
                    c1.Parameters.AddWithValue("@createdby", sale.createdby);

                    c1.ExecuteNonQuery();
                    int saleId = newSid;
                    
                    // insert each saledetails
                    //string id = saleId.ToString();
                    foreach (var item in sale.SaleDetails)
                    {
                        string q2 = @"
                                    insert into saledetails(sid,pid,quantity,rate,price,total)
                                    values(@Sid,@Pid,@Quantity,@Rate,@Price,@Total)
                                     ";
                        SqlCommand c2 = new SqlCommand(q2, con, tran);

                        c2.Parameters.AddWithValue("@Sid", saleId);
                        c2.Parameters.AddWithValue("@Pid", item.Pid);
                        c2.Parameters.AddWithValue("@Quantity", item.Quantity);
                        c2.Parameters.AddWithValue("@Rate", item.Rate);
                        c2.Parameters.AddWithValue("@Price", item.Price);
                        c2.Parameters.AddWithValue("@Total", item.Total);

                        c2.ExecuteNonQuery();
                    }
                    tran.Commit();
                    return "Invoice created successfully";
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    return $"Invoice creation failed: {ex.Message}";
                    //throw;

                }
            }
        }


        //get all invoices
        public List<Sale> getallinvoices()
        {
            List<Sale> invoices = new List<Sale>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "select * from sales";
                SqlCommand cmd = new SqlCommand(query, con);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        invoices.Add(new Sale
                        {
                            Sid = (int)reader["sid"],
                            Sdate = (DateTime)reader["sdate"],
                            Cid = (int)reader["cid"],
                            Totalmnt = (decimal)reader["totalmnt"],
                            invoicedate = reader["invoicedate"] == DBNull.Value ? null : (DateTime?)reader["invoicedate"],
                            createdat = (DateTime)reader["createdat"],
                            createdby = reader["createdby"].ToString()!,
                            SaleDetails = new List<SaleDetail>()
                        });
                    }
                }


                //get saleDetails 

                foreach(var sale in invoices)
                {
                    sale.SaleDetails = _saleDetailRepo.GetSaleDetail(sale.Sid, con);
                }

            }
            return invoices;
        }




        // Update Sale (Master + Details)
        public string UpdateSale(Sale sales)
        {
            var existing = GetSaleById(sales.Sid);
            if (existing == null) 
                return "Sale not found";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    // 1. Update Sale Master record (Preserve logic)
                    string updateMasterQuery = @"UPDATE sales SET 
                                        sdate = @Sdate, 
                                        cid = @Cid, 
                                        totalmnt = @Totalmnt, 
                                        invoicedate = @Invoicedate,
                                        modifiedat = @Modifiedat,
                                        modifiedby = @Modifiedby
                                        WHERE sid = @Sid";

                    SqlCommand cmdMaster = new SqlCommand(updateMasterQuery, con, tran);
                    cmdMaster.Parameters.AddWithValue("@Sid", sales.Sid);

                    // If incoming date is default, keep existing
                    cmdMaster.Parameters.AddWithValue("@Sdate", sales.Sdate == default ? existing.Sdate : sales.Sdate);
                    cmdMaster.Parameters.AddWithValue("@Cid", sales.Cid <= 0 ? existing.Cid : sales.Cid);
                    cmdMaster.Parameters.AddWithValue("@Totalmnt", sales.Totalmnt <= 0 ? existing.Totalmnt : sales.Totalmnt);
                    cmdMaster.Parameters.AddWithValue("@Invoicedate", (object?)sales.invoicedate ?? (object?)existing.invoicedate ?? DBNull.Value);
                    cmdMaster.Parameters.AddWithValue("@Modifiedat", DateTime.Now);
                    cmdMaster.Parameters.AddWithValue("@Modifiedby", sales.modifiedby ?? "John");

                    cmdMaster.ExecuteNonQuery();

                    // 2. If new details are provided, refresh the SaleDetails list
                    if (sales.SaleDetails != null && sales.SaleDetails.Any())
                    {
                        // Delete old details first
                        string deleteDetailsQuery = "DELETE FROM saledetails WHERE sid = @Sid";
                        SqlCommand cmdDel = new SqlCommand(deleteDetailsQuery, con, tran);
                        cmdDel.Parameters.AddWithValue("@Sid", sales.Sid);
                        cmdDel.ExecuteNonQuery();

                        // Insert new details
                        foreach (var item in sales.SaleDetails)
                        {
                            string insertDetailQuery = @"INSERT INTO saledetails (sid, pid, quantity, rate, price, total) 
                                                VALUES (@Sid, @Pid, @Quantity, @Rate, @Price, @Total)";
                            SqlCommand cmdDetail = new SqlCommand(insertDetailQuery, con, tran);
                            cmdDetail.Parameters.AddWithValue("@Sid", sales.Sid);
                            cmdDetail.Parameters.AddWithValue("@Pid", item.Pid);
                            cmdDetail.Parameters.AddWithValue("@Quantity", item.Quantity);
                            cmdDetail.Parameters.AddWithValue("@Rate", item.Rate);
                            cmdDetail.Parameters.AddWithValue("@Price", item.Price);
                            cmdDetail.Parameters.AddWithValue("@Total", item.Total);
                            cmdDetail.ExecuteNonQuery();
                        }
                    }

                    tran.Commit();
                    return "Sale updated successfully";
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return $"Update failed: {ex.Message}";
                }
            }
        }

        // Delete Sale (Master + Details)
        public bool DeleteSale(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    // 1. Delete details first (Referential Integrity)
                    string deleteDetails = "DELETE FROM saledetails WHERE sid = @Sid";
                    SqlCommand cmdDetails = new SqlCommand(deleteDetails, con, tran);
                    cmdDetails.Parameters.AddWithValue("@Sid", id);
                    cmdDetails.ExecuteNonQuery();

                    // 2. Delete Master record
                    string deleteMaster = "DELETE FROM sales WHERE sid = @Sid";
                    SqlCommand cmdMaster = new SqlCommand(deleteMaster, con, tran);
                    cmdMaster.Parameters.AddWithValue("@Sid", id);
                    int rows = cmdMaster.ExecuteNonQuery();

                    tran.Commit();
                    return rows > 0;
                }
                catch
                {
                    tran.Rollback();
                    return false;
                }
            }
        }



    }
}

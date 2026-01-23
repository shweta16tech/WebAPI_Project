using inventory.Models;
using Microsoft.Data.SqlClient;

namespace inventory.Repository
{
    public class RUser
    {
        private readonly string connectionString;
        public RUser(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        
        //get all users
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM users";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Uid = (int)reader["uid"],
                        Uname = reader["uname"].ToString()!,
                        Uemail = reader["uemail"].ToString(),
                        Upass = reader["upass"].ToString()!
                    });
                }
                return users;
            }
        }
        
    }
}

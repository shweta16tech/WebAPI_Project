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
                        UpassH = reader["upass"].ToString()!
                    });
                }
                return users;
            }
        }


        //add user
        public void AddUser(User user)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO users (uid,uname,uemail,upass)" +
                    "VALUES(@Uid,@Uname,@Uemail,@Upass)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Uid", user.Uid);
                cmd.Parameters.AddWithValue("@Uname", user.Uname);
                cmd.Parameters.AddWithValue("@Uemail",(object?)user.Uemail?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Upass", user.UpassH);

                con.Open();              
                cmd.ExecuteNonQuery();
            }
        }
        

        //
        public User? GetUserByName(string uname)
        {
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM users WHERE uname=@Uname";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Uname", uname);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    return new User
                    {
                        Uid = (int)reader["uid"],
                        Uname = reader["uname"].ToString()!,
                        Uemail = reader["uemail"].ToString(),
                        UpassH = reader["upass"].ToString()!
                    };
                }
                return null;
            }
        }
    }
}

using inventory.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
namespace inventory.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly string connectionString;

        public UserController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IActionResult GetAllUsers()
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
            }
            return Ok(users);
        }


        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}

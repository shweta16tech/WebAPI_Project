using inventory.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using inventory.Repository;
using inventory.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
namespace inventory.Controllers
{
    [Authorize]

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly RUser _repo;
        private readonly IConfiguration _configuration;

        public UserController(RUser repo,IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
        }

        //get all users
        [HttpGet]
        public IActionResult GetAllUsers()
        {

            var users = _repo.GetAllUsers()
                        .Select(u => new
                        {
                            u.Uid,
                            u.Uname,
                            u.Uemail
                        });
            return Ok(users);
        }

        //post user
        [HttpPost]
        public IActionResult CreateUser(CreateUserDto dto)
        {
            var existingUser = _repo.GetUserByName(dto.Uname);
            if (existingUser != null)
                return BadRequest("Username already exists.Please choose a different one"); 

         
            var user = new User
            {
                Uid = dto.Uid,
                Uname = dto.Uname,
                createdby = "John"
            };
            //now hashing the password
            var hasher = new PasswordHasher<User>();
            user.UpassH = hasher.HashPassword(user, dto.Upass);

            //Save in db
            _repo.AddUser(user);
            return Ok("User added successfully");

        }



        //login
        [HttpPost()]
        [Route("Login")]
        [AllowAnonymous]   

        public IActionResult Login(LoginDto dto)
        {
            var user = _repo.GetUserByName(dto.Uname);

            if (user == null)
                return Unauthorized("Invalid username or password");

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(
                                user,
                                user.UpassH,
                                dto.Upass
                                );
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid username or password");

            //Generate JWT
            var token = Helpers.JwtHelper.GenerateToken(user, _configuration);
            
            return Ok(new {Token=token,Username=user.Uname});
           
        }




        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}

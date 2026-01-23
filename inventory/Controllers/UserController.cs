using inventory.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using inventory.Repository;
namespace inventory.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly RUser _repo;

        public UserController(RUser repo)
        {
            _repo = repo;
        }

        //get all users

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _repo.GetAllUsers();
            return Ok(users);
        }


        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}

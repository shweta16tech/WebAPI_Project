using inventory.Models;
using inventory.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace inventory.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly RCustomer _repo;

        public CustomerController(RCustomer repo)
        {
            _repo = repo;
        }

        //GET: api/User
        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            var customers = _repo.GetAllCustomers();
            return Ok(customers);
        }


        //GET: api/Customer  get customer by id
        [HttpGet("{id}")]
        public IActionResult GetCustomerById(int id)
        {
            var customer = _repo.GetCustomerById(id);
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }


        //POST:api customer

        [HttpPost]
        public IActionResult AddCustomer(Customer customer)
        {
            _repo.AddCustomer(customer);
            return Ok("User added successfully");
        }
    }
}



//public IActionResult Index()
//{
//    return View();
//}
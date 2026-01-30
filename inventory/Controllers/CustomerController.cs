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
        public IActionResult AddCustomer([FromBody] Customer customer)
        {
            if (customer == null)
                return BadRequest("Customer information is missing or invalid.");
            customer.createdby = "John";
            _repo.AddCustomer(customer);
            return Ok("User added successfully");
        }



        // PUT: api/Customer/5
        // PUT: Customer/203
        [HttpPut()]
        public IActionResult UpdateCustomer( [FromBody] Customer customer)
        {
            if (customer == null) 
                return BadRequest();

            var checkExisting = _repo.GetCustomerById(customer.Cid);
            if (checkExisting == null) 
                return NotFound($"Customer does not exist.");

            // You can set the modifier here
            customer.modifiedby = "AdminUser";

            _repo.UpdateCustomer( customer);

            return Ok("Customer updated successfully.");
        }



        // DELETE: Customer/203
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            bool isDeleted = _repo.DeleteCustomer(id);

            if (!isDeleted)
            {
                return NotFound(new { message = $"Customer with ID {id} not found." });
            }

            return Ok(new { message = "Customer deleted successfully." });
        }

    }
}
using inventory.Models;
using inventory.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly RProduct _repo;

        public ProductController(RProduct repo)
        {
            _repo = repo;
        }

        //get all products
        //GET: api/Product
        [HttpGet]

        public IActionResult GetAllProducts()
        {
            var products = _repo.GetAllProducts();
            return Ok(products);
        }


        //Get product by ID

        [HttpGet("{id}")]
        public IActionResult GetProductByID(int id)
        {
            var product = _repo.GetProductByID( id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        //add new product
        //Post product

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            if (product == null)
                return BadRequest("Product information is missing or invalid.");
            product.createdby = "John";
            _repo.AddProduct(product);
            return Ok("Product added successfully");
        }




        // PUT: api/Product/1
        [HttpPut()]
        public IActionResult UpdateProduct([FromBody] Product product)
        {
            if (product == null) 
                return BadRequest();

            var existing = _repo.GetProductByID(product.Pid);
            if (existing == null) 
                return NotFound($"Product with ID {product.Pid} not found.");

            product.modifiedby = "John"; 
            _repo.UpdateProduct(product);

            return Ok("Product updated successfully.");
        }

        // DELETE: api/Product/1
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            bool isDeleted = _repo.DeleteProduct(id);
            if (!isDeleted)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            return Ok("Product deleted successfully.");
        }





    }
}


//public IActionResult Index()
//{
//    return View();
//}
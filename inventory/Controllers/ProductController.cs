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
            _repo.AddProduct(product);
            return Ok("Product added successfully");
        } 
    }
}


//public IActionResult Index()
//{
//    return View();
//}
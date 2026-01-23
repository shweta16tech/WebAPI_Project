using inventory.Models;
using inventory.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
namespace inventory.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SaleController : Controller
    {
        private readonly RSale _repo;
        public SaleController(RSale repo)
        {
            _repo = repo;
        }

        //get sales

        [HttpGet]
        public IActionResult GetAllSales()
        {
            var sales = _repo.GetAllSales();
            return Ok(sales);
        }


        //get sale by id 

        [HttpGet("{id}")]
        public IActionResult GetSaleById(int id)
        {
            var sale = _repo.GetSaleById(id);
            if (sale == null)
                return NotFound("Sale not found");
            return Ok(sale);
        }


        //post sale
        //POST:sale

        [HttpPost]
        public IActionResult AddSale(Sale sale)
        {
            _repo.AddSale(sale);
            return Ok("Sale added successfully");
        }
    }
}


//public IActionResult Index()
//{
//    return View();
//}
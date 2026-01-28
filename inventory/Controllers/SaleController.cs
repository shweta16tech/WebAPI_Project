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
        //get all sales
        [HttpGet]
        public IActionResult GetAllSales()
        {
            var sales = _repo.GetAllSales();
            return Ok(sales);
        }

        //get sales by pagination and filter

        [HttpGet("filtered")]
        public IActionResult GetSales(
            int pageNumber = 1,
            int pageSize = 5,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            int? pid = null,
            int? cid = null
            )
        {
            var sales = _repo.GetSales(
                pageNumber,
                pageSize,
                fromDate,
                toDate,
                pid,
                cid
                );
            return Ok(new
            {
                pageNumber,
                pageSize,
                count = sales.Count,
                data = sales


            });
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
            if (sale == null)
                return BadRequest("Sale information is missing or invalid.");
            sale.createdby = "John";
            _repo.AddSale(sale);
            return Ok("Sale added successfully");
        }
    }
}


//public IActionResult Index()
//{
//    return View();
//}
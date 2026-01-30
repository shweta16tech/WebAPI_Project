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
        //[HttpGet]
        //public IActionResult GetAllSales()
        //{
        //    var sales = _repo.GetAllSales();
        //    return Ok(sales);
        //}


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

        //post : sale/invoice
        [HttpPost()]
        public IActionResult createinvoice(Sale sale)
        {
            if (sale == null || sale.SaleDetails == null || !sale.SaleDetails.Any())
                return BadRequest("Invoice must contain at least one item");
            sale.createdby = "John";
            sale.createdat = DateTime.Now;

            //call repository and store the returned msg
            string resultMessage = _repo.createinvoice(sale);
            //check if msg indiccates success or failure
            if (resultMessage.StartsWith("Invoice created successfully"))
            {
                return Ok(new { message = resultMessage });
            }
            else
            {
                return BadRequest(new { message = resultMessage });
            }
        }

        //get sale invoices
        [HttpGet("invoices")]
        public IActionResult getallinvoices()
        {
            var invoices = _repo.getallinvoices();
            return Ok(invoices);
        }


        // PUT: Sale/1
        [HttpPut()]
        public IActionResult UpdateSale([FromBody] Sale sale)
        {
            if (sale == null) 
                return BadRequest("Invalid sale data.");

            string result = _repo.UpdateSale(sale);

            if (result.Contains("successfully"))
                return Ok(new { message = result });

            if (result == "Sale not found")
                return NotFound(result);

            return BadRequest(result);
        }

        // DELETE: Sale/1
        [HttpDelete("{id}")]
        public IActionResult DeleteSale(int id)
        {
            bool deleted = _repo.DeleteSale(id);
            if (!deleted) return NotFound("Sale record not found or could not be deleted.");

            return Ok(new { message = "Sale and its details deleted successfully." });
        }

    }
}
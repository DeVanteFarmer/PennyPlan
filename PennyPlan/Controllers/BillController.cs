using Microsoft.AspNetCore.Mvc;
using PennyPlan.Repositories;
using PennyPlan.Models;

namespace PennyPlan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IBillRepository _billRepository;

        public BillController(IBillRepository billRepository)
        {
            _billRepository = billRepository;
        }

        // GET: api/<BillController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_billRepository.GetAllUserBills());
        }

        [HttpGet("category/{categoryId}")]
        public IActionResult GetByCaterogyId(int categoryId)
        {
            var bills = _billRepository.GetBillsByCategory(categoryId);
            return Ok(bills);
        }

        // POST api/<BillController>
        [HttpPost]
        public IActionResult Post(Bill bill)
        {
            bill.CreatedAt = DateTime.Now;
            bill.UpdatedAt = DateTime.Now;
            bill.UserId = 1;
            _billRepository.AddBill(bill);
            return CreatedAtAction("Get", new { id = bill.Id }, bill);
        }

        // Put api/<BillController>
        [HttpPut("{id}")]
        public IActionResult Put(int id, Bill bill)
        {
            if (id != bill.Id)
            {
                return BadRequest();
            }

            _billRepository.Update(bill);
            return NoContent();
        }

        // Delete api/<BillController>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _billRepository.Delete(id);
            return NoContent();
        }

    }
}

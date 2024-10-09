using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using PennyPlan.Models;
using PennyPlan.Repositories;
using System.Security.Claims;

namespace PennyPlan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {

        private readonly ITransactionsRepository _transactionsRepository;

        public TransactionsController(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        [HttpGet("{userId}")]
        public IActionResult Get(int userId)
        {
            return Ok(_transactionsRepository.GetUserTransactions(userId));
        }

        // POST api/<TransactionsController>
        [HttpPost]
        public IActionResult Post(Transaction transaction)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from claims

            if (userId == null)
            {
                return Unauthorized(); // Return 401 if user is not authenticated
            }

            transaction.UserId = int.Parse(userId); // Convert userId to int
            transaction.CreatedAt = DateTime.Now;
            transaction.Date = DateTime.Now;

            _transactionsRepository.Add(transaction);

            return CreatedAtAction("Get", new { id = transaction.Id }, transaction);
        }


        // PUT api/<TransactionsController>5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Transaction transactions)
        {
            if (id != transactions.Id)
            {
                return BadRequest();
            }

            _transactionsRepository.Update(transactions);
            return NoContent();
        }

        // DELETE api/<TransactionsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _transactionsRepository.Delete(id);
            return NoContent();
        }
    }
}

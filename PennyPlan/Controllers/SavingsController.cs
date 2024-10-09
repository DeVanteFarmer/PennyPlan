using Microsoft.AspNetCore.Mvc;
using PennyPlan.Models;
using PennyPlan.Repositories;

namespace PennyPlan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavingsController : ControllerBase
    {
        private readonly ISavingsRepository _savingsRepository;

        public SavingsController(ISavingsRepository savingsRepository)
        {
            _savingsRepository = savingsRepository;
        }

        // POST: api/Savings (Add a new savings contribution)
        [HttpPost]
        public IActionResult Add(Savings savings)
        {
            _savingsRepository.Add(savings);
            return CreatedAtAction("Get", new { id = savings.Id }, savings);
        }

        // PUT: api/Savings/{id} (Update an existing savings contribution)
        [HttpPut("{id}")]
        public IActionResult Update(int id, Savings savings)
        {
            if (id != savings.Id)
            {
                return BadRequest();
            }

            _savingsRepository.Update(savings);
            return NoContent();
        }

        // DELETE: api/Savings/{id} (Delete a savings contribution)
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, int userId)
        {
            _savingsRepository.Delete(id, userId);
            return NoContent();
        }
    }
}


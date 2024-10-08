using Microsoft.AspNetCore.Mvc;
using PennyPlan.Repositories;
using PennyPlan.Models;

namespace PennyPlan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("GetByEmail")]
        public IActionResult GetByEmail(string email)
        {
            var user = _userRepository.GetByEmail(email);

            if (email == null || user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post(User user)
        {
            user.CreatedAt = DateTime.Now;
            _userRepository.Add(user);
            return CreatedAtAction(
                "GetByEmail",
                new { email = user.Email },
                user);
        }
    }
}

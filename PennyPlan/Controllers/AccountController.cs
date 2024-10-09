using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using PennyPlan.Repositories;
using PennyPlan.Models;

namespace PennyPlan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Register a user
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            if (_userRepository.GetByEmail(model.Email, model.Password) != null)
            {
                return BadRequest(new { message = "Email is already registered" });
            }

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
PasswordHash = model.Password,                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _userRepository.Add(user);

            return Ok(new { message = "User registered successfully" });
        }


        // Login a user
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = _userRepository.GetByEmail(model.Email, model.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            // Create user identity claims for authentication
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName)
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Sign in the user using cookie authentication
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = true // Set this to true if you want the cookie to persist across browser sessions
                });

            return Ok(new { message = "Login successful" });
        }
    }
}


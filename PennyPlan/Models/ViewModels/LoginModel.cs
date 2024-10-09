namespace PennyPlan.Models
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; } // Optional for persistent cookies
    }
}


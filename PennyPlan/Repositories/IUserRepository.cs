using PennyPlan.Models;

namespace PennyPlan.Repositories
{
    public interface IUserRepository
    {
        User GetByEmail(string email, string password);
        void Add(User user);
    }
}
using PennyPlan.Models;

namespace PennyPlan.Repositories
{
    public interface IUserRepository
    {
        User GetByEmail(string email);
        void Add(User user);
    }
}
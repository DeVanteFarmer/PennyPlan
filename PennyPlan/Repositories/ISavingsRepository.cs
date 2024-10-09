using PennyPlan.Models;

namespace PennyPlan.Repositories
{
    public interface ISavingsRepository
    {
        void Add(Savings savings);
        void Delete(int savingsId, int userId);
        Savings GetByUserId(int userId);
        void Update(Savings savings);
    }
}
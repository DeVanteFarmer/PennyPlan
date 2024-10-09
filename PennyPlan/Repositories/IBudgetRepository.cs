using PennyPlan.Models;

namespace PennyPlan.Repositories
{
    public interface IBudgetRepository
    {
        void Add(Budget budget);
        void Delete(int budgetId, int userId);
        void Update(Budget budget);
    }
}
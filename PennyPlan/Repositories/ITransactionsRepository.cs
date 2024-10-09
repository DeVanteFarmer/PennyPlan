using PennyPlan.Models;

namespace PennyPlan.Repositories
{
    public interface ITransactionsRepository
    {
        void Add(Transaction transaction);
        void Delete(int id);
        List<Transaction> GetUserTransactions(int userId);
        void Update(Transaction transaction);
    }
}
using PennyPlan.Models;

namespace PennyPlan.Repositories
{
    public interface IBillRepository
    {
        void AddBill(Bill bill);
        void Delete(int id);
        List<Bill> GetAllUserBills();
        List<Bill> GetBillsByCategory(int categoryId);
        void Update(Bill bill);
    }
}
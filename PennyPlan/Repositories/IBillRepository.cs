using PennyPlan.Models;

namespace PennyPlan.Repositories
{
    public interface IBillRepository
    {
        void AddBill(Bill bill);
        void Delete(int id);
        List<Bill> GetAllUserBills();
        List<Bill> GetBillsByCategoryId(int id);
        void Update(Bill bill);
    }
}
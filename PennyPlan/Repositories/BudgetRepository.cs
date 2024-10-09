using Microsoft.Data.SqlClient;
using PennyPlan.Models;
using PennyPlan.Utils;

namespace PennyPlan.Repositories
{
    public class BudgetRepository : BaseRepository, IBudgetRepository
    {
        private readonly ISavingsRepository _savingsRepository;

        public BudgetRepository(IConfiguration configuration, ISavingsRepository savingsRepository) : base(configuration)
        {
            _savingsRepository = savingsRepository;
        }

        // Method to Add a Budget
        public void Add(Budget budget)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Calculate total bills and savings
                    var totalBills = GetTotalBills(budget.UserId);
                    var totalSavings = _savingsRepository.GetByUserId(budget.UserId)?.SavingsAmount ?? 0;

                    // Calculate daily spending limit based on the 50/30/20 rule
                    var remainingIncome = budget.TotalIncome - totalBills - totalSavings;
                    var dailySpendingLimit = CalculateDailySpendingLimit(remainingIncome);

                    cmd.CommandText = @"
                    INSERT INTO Budget (UserId, TotalIncome, TotalBills, DailySpendingLimit, CreatedAt, UpdatedAt)
                    OUTPUT INSERTED.ID
                    VALUES (@UserId, @TotalIncome, @TotalBills, @DailySpendingLimit, @CreatedAt, @UpdatedAt)";

                    DbUtils.AddParameter(cmd, "@UserId", budget.UserId);
                    DbUtils.AddParameter(cmd, "@TotalIncome", budget.TotalIncome);
                    DbUtils.AddParameter(cmd, "@TotalBills", totalBills);
                    DbUtils.AddParameter(cmd, "@DailySpendingLimit", dailySpendingLimit);
                    DbUtils.AddParameter(cmd, "@CreatedAt", budget.CreatedAt);
                    DbUtils.AddParameter(cmd, "@UpdatedAt", budget.UpdatedAt);

                    budget.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        // Method to Update a Budget
        public void Update(Budget budget)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Recalculate the total bills
                    var totalBills = GetTotalBills(budget.UserId);

                    // Fetch the user's savings
                    var totalSavings = _savingsRepository.GetByUserId(budget.UserId)?.SavingsAmount ?? 0;

                    // Calculate the daily spending limit based on the 50/30/20 rule
                    var remainingIncome = budget.TotalIncome - totalBills - totalSavings;
                    var dailySpendingLimit = CalculateDailySpendingLimit(remainingIncome);

                    cmd.CommandText = @"
                UPDATE Budget
                SET TotalIncome = @TotalIncome,
                    TotalBills = @TotalBills, 
                    DailySpendingLimit = @DailySpendingLimit, 
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id AND UserId = @UserId";

                    DbUtils.AddParameter(cmd, "@TotalIncome", budget.TotalIncome);
                    DbUtils.AddParameter(cmd, "@TotalBills", totalBills); // Recalculate TotalBills on update
                    DbUtils.AddParameter(cmd, "@DailySpendingLimit", dailySpendingLimit); // Recalculate daily spending limit
                    DbUtils.AddParameter(cmd, "@UpdatedAt", DateTime.Now);
                    DbUtils.AddParameter(cmd, "@Id", budget.Id);
                    DbUtils.AddParameter(cmd, "@UserId", budget.UserId);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        // Method to Delete a Budget
        public void Delete(int budgetId, int userId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM Budget
                        WHERE Id = @Id AND UserId = @UserId";

                    DbUtils.AddParameter(cmd, "@Id", budgetId);
                    DbUtils.AddParameter(cmd, "@UserId", userId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Method to Calculate the TotalBills (Sum of all bills for the user)
        private int GetTotalBills(int userId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT SUM(Amount) AS TotalBills
                    FROM Bills
                    WHERE UserId = @UserId";

                    DbUtils.AddParameter(cmd, "@UserId", userId);

                    var result = cmd.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToInt32(result) : 0;
                }
            }
        }

        private int CalculateDailySpendingLimit(int remainingIncome)
        {
            int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            int discretionaryIncome = (int)(remainingIncome * 0.30); // 30% of remaining income for wants
            return discretionaryIncome / daysInMonth;
        }
    }
}

using Microsoft.Data.SqlClient;
using PennyPlan.Models;
using PennyPlan.Utils;

namespace PennyPlan.Repositories
{
    public class SavingsRepository : BaseRepository, ISavingsRepository
    {
        public SavingsRepository(IConfiguration configuration) : base(configuration) { }

        // Add savings contribution for a user
        public void Add(Savings savings)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Savings (UserId, SavingsAmount, CreatedAt, UpdatedAt)
                        OUTPUT INSERTED.ID
                        VALUES (@UserId, @SavingsAmount, @CreatedAt, @UpdatedAt)";

                    DbUtils.AddParameter(cmd, "@UserId", savings.UserId);
                    DbUtils.AddParameter(cmd, "@SavingsAmount", savings.SavingsAmount);
                    DbUtils.AddParameter(cmd, "@CreatedAt", savings.CreatedAt);
                    DbUtils.AddParameter(cmd, "@UpdatedAt", savings.UpdatedAt);

                    savings.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        // Get the user's savings
        public Savings GetByUserId(int userId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, UserId, SavingsAmount, CreatedAt, UpdatedAt
                        FROM Savings
                        WHERE UserId = @UserId";

                    DbUtils.AddParameter(cmd, "@UserId", userId);

                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new Savings()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            UserId = DbUtils.GetInt(reader, "UserId"),
                            SavingsAmount = DbUtils.GetInt(reader, "SavingsAmount"),
                            CreatedAt = DbUtils.GetDateTime(reader, "CreatedAt"),
                            UpdatedAt = DbUtils.GetDateTime(reader, "UpdatedAt")
                        };
                    }

                    return null;
                }
            }
        }

        // Update savings contribution
        public void Update(Savings savings)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Savings
                        SET SavingsAmount = @SavingsAmount, 
                            UpdatedAt = @UpdatedAt
                        WHERE Id = @Id AND UserId = @UserId";

                    DbUtils.AddParameter(cmd, "@SavingsAmount", savings.SavingsAmount);
                    DbUtils.AddParameter(cmd, "@UpdatedAt", DateTime.Now);
                    DbUtils.AddParameter(cmd, "@Id", savings.Id);
                    DbUtils.AddParameter(cmd, "@UserId", savings.UserId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Delete savings contribution
        public void Delete(int savingsId, int userId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM Savings
                        WHERE Id = @Id AND UserId = @UserId";

                    DbUtils.AddParameter(cmd, "@Id", savingsId);
                    DbUtils.AddParameter(cmd, "@UserId", userId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}


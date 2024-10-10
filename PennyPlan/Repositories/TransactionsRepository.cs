using PennyPlan.Models;
using PennyPlan.Utils;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PennyPlan.Repositories
{
    public class TransactionsRepository : BaseRepository, ITransactionsRepository
    {
        public TransactionsRepository(IConfiguration configuration) : base(configuration) { }

        public List<Transaction> GetUserTransactions(int userId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT t.Id, t.UserId, t.TransactionName, t.Amount, t.CategoryId, t.Date, t.CreatedAt, 
                       u.Id AS UserId, u.UserName, u.Email, c.Name AS CategoryName
                FROM Transactions t
                     LEFT JOIN Categories c ON t.CategoryId = c.Id
                     LEFT JOIN [Users] u ON t.UserId = u.Id
                WHERE t.UserId = @userId
                ORDER BY t.CreatedAt DESC";

                    cmd.Parameters.AddWithValue("@userId", userId);

                    var reader = cmd.ExecuteReader();
                    var transactions = new List<Transaction>();

                    while (reader.Read())
                    {
                        var transaction = new Transaction()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            UserId = DbUtils.GetInt(reader, "UserId"),
                            TransactionName = DbUtils.GetString(reader, "TransactionName"),
                            Amount = DbUtils.GetInt(reader, "Amount"),
                            CategoryId = DbUtils.GetInt(reader, "CategoryId"),
                            Date = DbUtils.GetDateTime(reader, "Date"),
                            CreatedAt = DbUtils.GetDateTime(reader, "CreatedAt"),
                            Category = new Category
                            {
                                Id = DbUtils.GetInt(reader, "CategoryId"),
                                Name = DbUtils.GetString(reader, "CategoryName")
                            },
                            User = new User()
                            {
                                Id = DbUtils.GetInt(reader, "UserId"),
                                UserName = DbUtils.GetString(reader, "UserName"),
                                Email = DbUtils.GetString(reader, "Email")
                            }
                        };
                        transactions.Add(transaction);
                    }
                    reader.Close();
                    return transactions;
                }
            }
        }

        public void Add(Transaction transaction)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Transactions (UserId, TransactionName, Amount, CategoryId, Date, CreatedAt)
                        OUTPUT INSERTED.ID 
                        VALUES (@UserId, @TransactionName, @Amount, @CategoryId, @Date, @CreatedAt)";

                    DbUtils.AddParameter(cmd, "@UserId", transaction.UserId);
                    DbUtils.AddParameter(cmd, "@TransactionName", transaction.TransactionName);
                    DbUtils.AddParameter(cmd, "@Amount", transaction.Amount);
                    DbUtils.AddParameter(cmd, "@CategoryId", transaction.CategoryId);
                    DbUtils.AddParameter(cmd, "@Date", transaction.Date);
                    DbUtils.AddParameter(cmd, "@CreatedAt", transaction.CreatedAt);

                    transaction.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Transaction transaction)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Transactions
                           SET TransactionName = @TransactionName,
                               Amount = @Amount,
                               CategoryId = @CategoryId,
                               Date = @Date,
                               UpdatedAt = @UpdatedAt
                        WHERE Id = @Id";

                    DbUtils.AddParameter(cmd, "@TransactionName", transaction.TransactionName);
                    DbUtils.AddParameter(cmd, "@Amount", transaction.Amount);
                    DbUtils.AddParameter(cmd, "@CategoryId", transaction.CategoryId);
                    DbUtils.AddParameter(cmd, "@Date", transaction.Date);
                    DbUtils.AddParameter(cmd, "@UpdatedAt", DateTime.Now);
                    DbUtils.AddParameter(cmd, "@Id", transaction.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Transactions WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}


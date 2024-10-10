using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using PennyPlan.Models;
using PennyPlan.Utils;
using System.Collections.Generic;

namespace PennyPlan.Repositories
{
    public class BillRepository : BaseRepository, IBillRepository
    {
        public BillRepository(IConfiguration configuration) : base(configuration) { }

        public List<Bill> GetAllUserBills()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT b.Id, b.UserId, b.BillName, b.Amount, b.DueDate, 
                       b.Paid, b.CreatedAt, b.UpdatedAt,  
                       u.[Id] AS UserId, u.UserName, u.Email, u.CreatedAt AS UserCreatedAt,
                       u.UpdatedAt AS UserUpdatedAt, u.PasswordHash,
                       c.Id AS CategoryId, c.Name AS CategoryName
                FROM Bills b
                     LEFT JOIN [Users] u ON b.UserId = u.Id
                     LEFT JOIN [Categories] c ON b.CategoryId = c.Id
                ORDER BY b.Paid DESC";

                    var reader = cmd.ExecuteReader();
                    var bills = new List<Bill>();
                    while (reader.Read())
                    {
                        bills.Add(NewBillFromReader(reader));
                    }

                    reader.Close();
                    return bills;
                }
            }
        }


        public List<Bill> GetBillsByCategoryId(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT b.Id, b.UserId, b.BillName, b.Amount, b.DueDate, 
                       b.Paid, b.CreatedAt, b.UpdatedAt, 
                       c.Id AS CategoryId, c.Name AS CategoryName
                FROM Bills b
                     LEFT JOIN Categories c ON b.CategoryId = c.Id
                WHERE b.CategoryId = @CategoryId
                ORDER BY b.BillName ASC";

                    cmd.Parameters.AddWithValue("@CategoryId", id);

                    var reader = cmd.ExecuteReader();
                    var bills = new List<Bill>();

                    while (reader.Read())
                    {
                        bills.Add(new Bill
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            BillName = reader.GetString(reader.GetOrdinal("BillName")),
                            Amount = reader.GetInt32(reader.GetOrdinal("Amount")),
                            DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                            Paid = reader.GetBoolean(reader.GetOrdinal("Paid")),
                            Category = new Category
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                Name = reader.GetString(reader.GetOrdinal("CategoryName"))
                            }
                        });
                    }

                    reader.Close();
                    return bills;
                }
            }
        }


        // CRUD Methods Below //
        // Add a new bill
        public void AddBill(Bill bill)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
        INSERT INTO Bills (UserId, BillName, Amount, DueDate, Paid, CategoryId, CreatedAt, UpdatedAt)
        OUTPUT INSERTED.ID
        VALUES (@UserId, @BillName, @Amount, @DueDate, @Paid, @CategoryId, @CreatedAt, @UpdatedAt)";

                    DbUtils.AddParameter(cmd, "@UserId", bill.UserId);
                    DbUtils.AddParameter(cmd, "@BillName", bill.BillName); // BillName acts as category
                    DbUtils.AddParameter(cmd, "@Amount", bill.Amount);
                    DbUtils.AddParameter(cmd, "@DueDate", bill.DueDate);
                    DbUtils.AddParameter(cmd, "@Paid", bill.Paid);
                    DbUtils.AddParameter(cmd, "@CategoryId", bill.Category.Id); // Insert CategoryId
                    DbUtils.AddParameter(cmd, "@CreatedAt", bill.CreatedAt);
                    DbUtils.AddParameter(cmd, "@UpdatedAt", bill.UpdatedAt);

                    bill.Id = (int)cmd.ExecuteScalar();
                }
            }
        }


        // Update an existing bill
        public void Update(Bill bill)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
        UPDATE Bills
        SET BillName = @BillName, 
            Amount = @Amount, 
            DueDate = @DueDate, 
            Paid = @Paid,
            CategoryId = @CategoryId,
            UpdatedAt = @UpdatedAt
        WHERE Id = @Id AND UserId = @UserId";

                    DbUtils.AddParameter(cmd, "@BillName", bill.BillName); // BillName acts as category
                    DbUtils.AddParameter(cmd, "@Amount", bill.Amount);
                    DbUtils.AddParameter(cmd, "@DueDate", bill.DueDate);
                    DbUtils.AddParameter(cmd, "@Paid", bill.Paid);
                    DbUtils.AddParameter(cmd, "@CategoryId", bill.Category.Id); // Update CategoryId
                    DbUtils.AddParameter(cmd, "@UpdatedAt", DateTime.Now);
                    DbUtils.AddParameter(cmd, "@Id", bill.Id);
                    DbUtils.AddParameter(cmd, "@UserId", bill.UserId);

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
                    cmd.CommandText = "DELETE FROM Bill WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // CRUD Methods Above //

        private Bill NewBillFromReader(SqlDataReader reader)
        {
            return new Bill()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                BillName = reader.GetString(reader.GetOrdinal("BillName")),
                Amount = reader.GetInt32(reader.GetOrdinal("Amount")),
                DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                Paid = reader.GetBoolean(reader.GetOrdinal("Paid")),
                Category = new Category
                {
                    Id = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                    Name = reader.GetString(reader.GetOrdinal("CategoryName"))
                },
                User = new User()
                {
                    Id = DbUtils.GetInt(reader, "UserId"),
                    UserName = DbUtils.GetString(reader, "UserName"),
                    PasswordHash = DbUtils.GetString(reader, "PasswordHash"),
                    Email = DbUtils.GetString(reader, "Email"),
                    CreatedAt = DbUtils.GetDateTime(reader, "UserCreatedAt"),
                    UpdatedAt = DbUtils.GetDateTime(reader, "UserUpdatedAt")
                }
            };
        }

    }
}


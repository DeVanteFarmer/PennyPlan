﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using PennyPlan.Models;
using PennyPlan.Utils;



namespace PennyPlan.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(IConfiguration configuration) : base(configuration) { }

        public List<Category> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = " SELECT Id, Name FROM Category ORDER BY Name ASC";
                    var reader = cmd.ExecuteReader();

                    var categories = new List<Category>();
                    while (reader.Read())

                    {
                        categories.Add(new Category()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            Name = DbUtils.GetString(reader, "Name"),
                        });
                    }
                    reader.Close();
                    return categories;
                }

            }
        }

        public Category GetCategoryById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"SELECT * FROM Category WHERE Id = @id";
                        DbUtils.AddParameter(cmd, "@Id", id);

                        var reader = cmd.ExecuteReader();


                        if (reader.Read())
                        {
                            Category category = new Category
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                Name = DbUtils.GetString(reader, "Name"),
                            };
                            reader.Close();
                            return category;

                        }
                        reader.Close();
                        return null;
                    }
                }
            }
        }
        public void Add(Category category)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                INSERT INTO Category (Name, IsBillCategory, IsTransactionCategory)
                OUTPUT INSERTED.ID
                VALUES (@Name, @IsBillCategory, @IsTransactionCategory)";

                    DbUtils.AddParameter(cmd, "@Name", category.Name);
                    DbUtils.AddParameter(cmd, "@IsBillCategory", category.IsBillCategory);
                    DbUtils.AddParameter(cmd, "@IsTransactionCategory", category.IsTransactionCategory);

                    category.Id = (int)cmd.ExecuteScalar();
                }
            }
        }


        public void Update(Category category)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                UPDATE Category
                   SET Name = @Name,
                       IsBillCategory = @IsBillCategory,
                       IsTransactionCategory = @IsTransactionCategory
                WHERE Id = @Id";

                    DbUtils.AddParameter(cmd, "@Name", category.Name);
                    DbUtils.AddParameter(cmd, "@IsBillCategory", category.IsBillCategory);
                    DbUtils.AddParameter(cmd, "@IsTransactionCategory", category.IsTransactionCategory);
                    DbUtils.AddParameter(cmd, "@Id", category.Id);

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
                    cmd.CommandText = "DELETE FROM Category WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

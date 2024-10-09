using Microsoft.Data.SqlClient;
using PennyPlan.Models;
using PennyPlan.Utils;
using BCrypt.Net;
using System.Text;


namespace PennyPlan.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration) { }

        public User GetByEmail(string email, string password)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT u.Id, u.UserName, u.Email, u.CreatedAt,
                               u.UpdatedAt, u.PasswordHash
                          FROM [User] u
                         WHERE u.Email = @email";

                    DbUtils.AddParameter(cmd, "@email", email);

                    User user = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string storedPasswordHash = DbUtils.GetString(reader, "PasswordHash");

                        // Verify the password using bcrypt
                        if (BCrypt.Net.BCrypt.Verify(password, storedPasswordHash))
                        {
                            user = new User()
                            {
                                Id = DbUtils.GetInt(reader, "Id"),
                                UserName = DbUtils.GetString(reader, "UserName"),
                                PasswordHash = storedPasswordHash,
                                Email = DbUtils.GetString(reader, "Email"),
                                CreatedAt = DbUtils.GetDateTime(reader, "CreatedAt"),
                                UpdatedAt = DbUtils.GetDateTime(reader, "UpdatedAt")
                            };
                        }
                    }
                    reader.Close();

                    return user; // Will return null if email not found or password is incorrect
                }
            }
        }

        // Method to hash the password before storing it
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string enteredPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
        }

        public void Add(User user)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Hash the password before storing it
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

                    cmd.CommandText = @"
                INSERT INTO [User] (UserName, Email, PasswordHash, CreatedAt, UpdatedAt)
                OUTPUT INSERTED.ID
                VALUES (@UserName, @Email, @PasswordHash, @CreatedAt, @UpdatedAt)";

                    DbUtils.AddParameter(cmd, "@UserName", user.UserName);
                    DbUtils.AddParameter(cmd, "@Email", user.Email);
                    DbUtils.AddParameter(cmd, "@PasswordHash", user.PasswordHash); // hashed now
                    DbUtils.AddParameter(cmd, "@CreatedAt", user.CreatedAt);
                    DbUtils.AddParameter(cmd, "@UpdatedAt", user.UpdatedAt);

                    user.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

    }
}

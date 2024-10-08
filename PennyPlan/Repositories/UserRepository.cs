using Microsoft.Data.SqlClient;
using PennyPlan.Models;
using PennyPlan.Utils;


namespace PennyPlan.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration) { }

        public User GetByEmail(string email)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT u.Id, u.UserName, u.Email, u.CreatedAt,
                               u.UpdatedAt, u.PasswordHash
                          FROM User u
                         WHERE Email = @email";

                    DbUtils.AddParameter(cmd, "@email", email);

                    User user = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        user = new User()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            UserName = DbUtils.GetString(reader, "UserName"),
                            PasswordHash = DbUtils.GetString(reader, "PasswordHash"),
                            Email = DbUtils.GetString(reader, "Email"),
                            CreatedAt = DbUtils.GetDateTime(reader, "CreatedAt"),
                            UpdatedAt = DbUtils.GetDateTime(reader, "UpdatedAt"),
                        };
                    }
                    reader.Close();

                    return user;
                }
            }
        }

        public void Add(User user)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO User (UserName, Email, 
                                      PasswordHash, CreatedAt, UpdatedAt)
                                        OUTPUT INSERTED.ID
                                        VALUES (@UserName, @Email, @PasswordHash, 
                                                @CreatedAt, @UpdatedAt, 0)";
                    DbUtils.AddParameter(cmd, "@UserName", user.UserName);
                    DbUtils.AddParameter(cmd, "@Email", user.Email);
                    DbUtils.AddParameter(cmd, "@PasswordHash", user.PasswordHash);
                    DbUtils.AddParameter(cmd, "@Email", user.Email);
                    DbUtils.AddParameter(cmd, "@CreatedAt", user.CreatedAt);
                    DbUtils.AddParameter(cmd, "@UpdatedAt", user.UpdatedAt);

                    user.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}

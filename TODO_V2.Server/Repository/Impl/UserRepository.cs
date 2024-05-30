using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using System.Threading.Tasks;
using TODO_V2.Server.Repository.Interfaces;
using TODO_V2.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TODO_V2.Server.Models;

namespace TODO_V2.Server.Repository.Impl
{
    public class UserRepository : IUserRepository
    { 
        private readonly IConfiguration _configuration;
        private string ConnectionString => _configuration.GetConnectionString("TODO_V2DB");

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public async Task<bool> Add(User user, UserCredentials userCredentials)
        {
            using (var dbConnection = CreateConnection())
            {
                var userId = await dbConnection.ExecuteScalarAsync<int>(@"
                    INSERT INTO Users (UserName, Name, Surname, UserType) 
                    VALUES (@UserName, @Name, @Surname, @UserType); 
                    SELECT SCOPE_IDENTITY()", user);


                userCredentials.UserId = userId;

                await dbConnection.ExecuteAsync(@"
                    INSERT INTO UserCredentials (UserId, UserName, EncryptedPassword) 
                    VALUES (@UserId, @UserName, @EncryptedPassword)", userCredentials);
            }
            return true;
        }


        public async Task<User> Update(User user, UserCredentials userCredentials)
        {
            using (var dbConnection = CreateConnection())
            {
                await dbConnection.ExecuteAsync(@"
            UPDATE Users SET Name = @Name, Surname = @Surname, UserName = @UserName, UserType = @UserType, UpdatedAt = GETDATE() 
            WHERE Id = @Id", user);

                await dbConnection.ExecuteAsync(@"
            UPDATE UserCredentials SET UserName = @UserName, EncryptedPassword = @EncryptedPassword, UpdatedAt = GETDATE() WHERE UserId = @UserId", userCredentials);
            }
            return user;
        }

        public async Task<bool> Delete(int id)
        {
            using (var dbConnection = CreateConnection())
            {
                await dbConnection.ExecuteAsync($"DELETE FROM UserCredentials WHERE UserId = {id}");
                await dbConnection.ExecuteAsync($"DELETE FROM Users WHERE Id = {id}");
                
            }
            return true;
        }

        public async Task<bool> LogicDelete(int id)
        {
            using (var dbConnection = CreateConnection())
            {
                await dbConnection.ExecuteAsync("UPDATE Users SET IsDeleted = 1, DeletedAt = GETDATE() WHERE Id = @Id", new { Id = id });
                await dbConnection.ExecuteAsync("UPDATE UserCredentials SET IsDeleted = 1, DeletedAt = GETDATE() WHERE UserId = @Id", new { Id = id });
            }
            return true;
        }

        public async Task<IEnumerable<User>> GetAll(GetRequest<User> request)
        {
            using (var dbConnection = CreateConnection())
            {
                return await dbConnection.QueryAsync<User>("SELECT * FROM Users");
            }
        }

        public async Task<IEnumerable<User>> GetAllLogic(GetRequest<User> request)
        {
            using (var dbConnection = CreateConnection())
            {
                return await dbConnection.QueryAsync<User>("SELECT * FROM Users WHERE IsDeleted = 0");
            }
        }

        public async Task<User?> GetById(int id)
        {
            using (var dbConnection = CreateConnection())
            {
                return await dbConnection.QueryFirstOrDefaultAsync<User>(
                    $"SELECT * FROM Users WHERE Id = {id} AND IsDeleted = 0");
            }
        }

        public async Task<User?> GetByUserName(string username)
        {
            using (var dbConnection = CreateConnection())
            {
                return await dbConnection.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM Users WHERE UserName = @UserName AND IsDeleted = 0", new { UserName = username });
            }
        }

        public async Task<UserCredentials> GetUserCredentialsByUserName(string username)
        {
            using (var dbConnection = CreateConnection())
            {
                return await dbConnection.QueryFirstOrDefaultAsync<UserCredentials>(
                    "SELECT * FROM UserCredentials WHERE UserName = @UserName AND IsDeleted = 0",
                    new { UserName = username });
            }
        }

        public async Task<UserCredentials> GetUserCredentialsById(int userId)
        {
            using (var dbConnection = CreateConnection())
            {
                return await dbConnection.QueryFirstOrDefaultAsync<UserCredentials>(
                    "SELECT * FROM UserCredentials WHERE UserId = @userId AND IsDeleted = 0",
                    new { UserId = userId });
            }
        }


        public async Task<int> Count()
        {
            try
            {
                using (var dbConnection = CreateConnection())
                {
                    const string query = "SELECT COUNT(*) FROM Users";
                    var count = await dbConnection.ExecuteScalarAsync<int>(query);
                    return count;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al contar usuarios: {ex.Message}");
                return 0;
            }
        }
    }
}

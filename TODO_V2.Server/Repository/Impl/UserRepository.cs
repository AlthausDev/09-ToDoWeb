using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using System.Threading.Tasks;
using TODO_V2.Server.Repository.Interfaces;
using TODO_V2.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace TODO_V2.Server.Repository.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TODO_V2DB");
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<bool> Add(User user)
        {
            using (var dbConnection = CreateConnection())
            {
                await dbConnection.ExecuteAsync(@"
                    INSERT INTO Users (Name, Surname, UserName, Password, UserType) 
                    VALUES (@Name, @Surname, @UserName, @Password, @UserType)", user);
            }
            return true;
        }

        public async Task<User> Update(User user)
        {
            using (var dbConnection = CreateConnection())
            {
                await dbConnection.ExecuteAsync(@"
                    UPDATE Users SET Name = @Name, Surname = @Surname, UserName = @UserName, 
                    Password = @Password, UserType = @UserType WHERE Id = @Id", user);
            }
            return user;
        }

        public async Task<bool> Delete(int id)
        {
            using (var dbConnection = CreateConnection())
            {
                await dbConnection.ExecuteAsync("DELETE FROM Users WHERE Id = @Id", new { Id = id });
            }
            return true;
        }

        public async Task<bool> LogicDelete(int id)
        {
            using (var dbConnection = CreateConnection())
            {
                await dbConnection.ExecuteAsync("UPDATE Users SET Deleted = 1 WHERE Id = @Id", new { Id = id });
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
                return await dbConnection.QueryAsync<User>("SELECT * FROM Users WHERE Deleted = 0");
            }
        }

        public async Task<User?> GetById(int id)
        {
            using (var dbConnection = CreateConnection())
            {
                return await dbConnection.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM Users WHERE Id = @Id AND Deleted = 0", new { Id = id });
            }
        }

        public async Task<User?> GetByUserName(string username)
        {
            using (var dbConnection = CreateConnection())
            {
                return await dbConnection.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM Users WHERE UserName = @UserName AND Deleted = 0", new { UserName = username });
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

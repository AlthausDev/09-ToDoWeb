using TODO_V2.Server.Repository.Interfaces;
using TODO_V2.Shared;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using TODO_V2.Shared.Models;

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


        public void Add(User user)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                try { 
                string query = @$"INSERT INTO Usuarios (UserName, Password, Name, UserType) 
                                VALUES ('{user.UserName}', '{user.Password}', '{user.Name}', '{user.UserType}');";
                dbConnection.Execute(query);
                }
                catch
                {

                }
            }
        }

        public IEnumerable<User> GetAllAdmin()
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @"SELECT * FROM Usuarios;";

                return dbConnection.Query<User>(query);
            }
        }


        public IEnumerable<User> GetAll()
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @"SELECT * FROM Usuarios WHERE Deleted = 0;";

                return dbConnection.Query<User>(query);
            }
        }

        public User GetById(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = $"SELECT * FROM Usuarios WHERE Id = {id} AND Deleted = 0;";

                User aux = dbConnection.QuerySingle<User>(query);
                User user = aux;
                return user;
            }
        }


        public void Remove(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = $"DELETE FROM Usuarios WHERE Id = {id};";

                dbConnection.Execute(query);
            }
        }

        public void LogicRemove(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = $"UPDATE Usuarios SET Deleted = 1 WHERE Id = {id};";

                dbConnection.Execute(query);
            }
        }

        public void Update(User user)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @$"UPDATE Usuarios SET Username = '{user.UserName}', Password ='{user.Password}', Name = '{user.Name}', UserType = '{user.UserType}' WHERE Id = '{user.Id}';";

                dbConnection.Execute(query);
            }
        }        
    }
}

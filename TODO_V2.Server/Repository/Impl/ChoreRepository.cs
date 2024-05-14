using TODO_V2.Server.Repository.Interfaces;
using TODO_V2.Shared;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using TODO_V2.Shared.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Data.Common;

namespace TODO_V2.Server.Repository.Impl
{
    public class ChoreRepository : IChoreRepository
    {
        private readonly IConfiguration _configuration;
        private string ConnectionString => _configuration.GetConnectionString("TODO_V2");

        public ChoreRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Add(Chore chore)
        {        
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @$"INSERT INTO Tasks (CategoryID, UserID, State, TaskName, ExpirationDate) VALUES ('{chore.CategoryID}', '{chore.UserID}', '{chore.State}', '{chore.TaskName}';";
                dbConnection.Query(query);
            }
        }

        public IEnumerable<Chore> GetAllAdmin()
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @"SELECT * FROM Tasks;";

                return dbConnection.Query<Chore>(query);
            }
        }

        public IEnumerable<Chore> GetAll()
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @"SELECT * FROM Tasks WHERE Deleted = 0;";
                return dbConnection.Query<Chore>(query);
            }
        }

        public Chore GetById(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = $"SELECT * FROM Tasks WHERE Id = {id};";
                return dbConnection.QueryFirstOrDefault<Chore>(query);
            }
        }

        public void Update(Chore chore)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @$"UPDATE Tasks SET CategoryID = '{chore.CategoryID}', UserID = '{chore.UserID}', State = '{chore.State}', TaskName = '{chore.TaskName}';";
                dbConnection.Execute(query);
            }
        }

        public void Remove(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = $"DELETE FROM Tasks WHERE Id = {id};";
                dbConnection.Execute(query);
            }
        }


        public void LogicRemove(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = $"UPDATE Tasks SET Deleted = 1 WHERE Id = {id};";

                dbConnection.Execute(query);
            }
        }
    }
}

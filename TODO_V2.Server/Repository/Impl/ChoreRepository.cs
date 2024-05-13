using TODO_V2.Server.Repository.Interfaces;
using TODO_V2.Shared;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Repository.Impl
{
    public class ChoreRepository : IChoreRepository
    {
        private readonly IConfiguration _configuration;
        private string ConnectionString => _configuration.GetConnectionString("TODO_V2DB");

        public ChoreRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Add(Chore chore)
        {        
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                //string query = @$"INSERT INTO Chore (Name, Descripcion, Finalizado) VALUES ('{chore.Name}', '{chore.Descripcion}', {(chore.Finalizado ? 1 : 0)});";
                //dbConnection.Query(query);
            }
        }

        public IEnumerable<Chore> GetAll()
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @"SELECT * FROM Chore;";
                return dbConnection.Query<Chore>(query);
            }
        }

        public Chore GetById(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = $"SELECT * FROM Chore WHERE Id = {id};";
                return dbConnection.QueryFirstOrDefault<Chore>(query);
            }
        }
      
        public void Remove(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = $"DELETE FROM Chore WHERE Id = {id};";
                dbConnection.Execute(query);
            }
        }


        public void Update(Chore chore)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                //string query = @$"UPDATE Chore SET Name = '{chore.Name}', Descripcion = '{chore.Descripcion}', Finalizado = '{(chore.Finalizado ? 1 : 0)}' WHERE Id = '{chore.Id}';";
                //dbConnection.Execute(query);
            }
        }

        public void LogicRemove(int id)
        {
            throw new NotImplementedException();
        }
    }
}

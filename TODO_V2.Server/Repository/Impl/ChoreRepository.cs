//using TODO_V2.Server.Repository.Interfaces;
//using TODO_V2.Shared;
//using Dapper;
//using Microsoft.Data.SqlClient;
//using System.Data;
//using TODO_V2.Shared.Models;
//using Microsoft.AspNetCore.Mvc;


//namespace TODO_V2.Server.Repository.Impl
//{
//    public class ChoreRepository : IChoreRepository
//    {
//        private readonly IConfiguration _configuration;
//        private string ConnectionString => _configuration.GetConnectionString("TODO_V2DB");

//        public ChoreRepository(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        public async Task<bool> Add(Chore chore)
//        {
//            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
//            {
//                try
//                {
//                    string query = @$"INSERT INTO Chores (CategoryID, UserID, State, TaskName, ExpirationDate) VALUES ('{chore.CategoryID}', '{chore.UserID}', '{chore.State}', '{chore.TaskName}', '{chore.ExpirationDate};";
//                    dbConnection.Execute(query);
//                }
//                catch (Exception) { }
//            }
//            return true;
//        }

//        public async Task<Chore> Update(Chore chore)
//        {
//            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
//            {
//                string query = @$"UPDATE Chores SET CategoryID = '{chore.CategoryID}', UserID = '{chore.UserID}', State = '{chore.State}', TaskName = '{chore.TaskName}', ExpirationDate = '{chore.ExpirationDate}';";

//                dbConnection.Execute(query);
//            }
//            return chore;
//        }

//        public void Delete(int id)
//        {
//            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
//            {
//                string query = $"DELETE FROM Chores WHERE Id = {id};";

//                dbConnection.Execute(query);
//            }
//        }

//        public void LogicDelete(int id)
//        {
//            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
//            {
//                string query = $"UPDATE Chores SET Deleted = 1 WHERE Id = {id};";

//                dbConnection.Execute(query);
//            }  
//        }

//        public async Task<IEnumerable<Chore>> GetAll(GetRequest<Chore> request)
//        {
//            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
//            {
//                string query = @"SELECT * FROM Chores;";

//                return dbConnection.Query<Chore>(query);
//            }
//            //IQueryable<T> query = _context.Set<T>();

//            //if (request.Filter != null)
//            //{
//            //    query = query.Where(request.Filter);
//            //}

//            //if (request.OrderBy != null)
//            //{
//            //    query = request.OrderBy(query);
//            //}

//            //if (request.Skip.HasValue)
//            //{
//            //    query = query.Skip(request.Skip.Value);
//            //}

//            //if (request.Take.HasValue)
//            //{
//            //    query = query.Take(request.Take.Value);
//            //}

//            //return query.ToList();
//        }


//        public async Task<IEnumerable<Chore>> GetAllLogic(GetRequest<Chore> request)
//        {
//            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
//            {
//                string query = @"SELECT * FROM Chores WHERE Deleted = 0;";

//                return dbConnection.Query<Chore>(query);
//            }

//            //IQueryable<T> query = _context.Set<T>();

//            //if (request.Filter != null)
//            //{
//            //    query = query.Where(request.Filter);
//            //}

//            //if (request.OrderBy != null)
//            //{
//            //    query = request.OrderBy(query);
//            //}

//            //if (request.Skip.HasValue)
//            //{
//            //    query = query.Skip(request.Skip.Value);
//            //}

//            //if (request.Take.HasValue)
//            //{
//            //    query = query.Take(request.Take.Value);
//            //}

//            //return query.ToList();
//        }

//        public async Task<Chore?> GetById(int id)
//        {
//            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
//            {
//                string query = $"SELECT * FROM Chores WHERE Id = {id} AND Deleted = 0;"; 
//                Chore chore = dbConnection.QuerySingle<Chore>(query);               

//                return chore;
//            }
//        }

//        public ActionResult<int> Count()
//        {
//            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
//            {
//                string query = $"SELECT COUNT(Id) FROM Chores";
//                int count = dbConnection.QuerySingle<int>(query);

//                return count;
//            }
//        }
   
//    }
//}

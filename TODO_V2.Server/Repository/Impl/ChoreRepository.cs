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

//        public async Task<bool> Add(TaskItem TaskItem)
//        {
//            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
//            {
//                try
//                {
//                    string query = @$"INSERT INTO Chores (CategoryID, UserID, State, TaskItemName, ExpirationDate) VALUES ('{TaskItem.CategoryID}', '{TaskItem.UserID}', '{TaskItem.State}', '{TaskItem.TaskItemName}', '{TaskItem.ExpirationDate};";
//                    dbConnection.Execute(query);
//                }
//                catch (Exception) { }
//            }
//            return true;
//        }

//        public async Task<TaskItem> Update(TaskItem TaskItem)
//        {
//            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
//            {
//                string query = @$"UPDATE Chores SET CategoryID = '{TaskItem.CategoryID}', UserID = '{TaskItem.UserID}', State = '{TaskItem.State}', TaskItemName = '{TaskItem.TaskItemName}', ExpirationDate = '{TaskItem.ExpirationDate}';";

//                dbConnection.Execute(query);
//            }
//            return TaskItem;
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

//        public async Task<IEnumerable<TaskItem>> GetAll(GetRequest<TaskItem> request)
//        {
//            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
//            {
//                string query = @"SELECT * FROM Chores;";

//                return dbConnection.Query<TaskItem>(query);
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


//        public async Task<IEnumerable<TaskItem>> GetAllLogic(GetRequest<TaskItem> request)
//        {
//            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
//            {
//                string query = @"SELECT * FROM Chores WHERE Deleted = 0;";

//                return dbConnection.Query<TaskItem>(query);
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

//        public async Task<TaskItem?> GetById(int id)
//        {
//            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
//            {
//                string query = $"SELECT * FROM Chores WHERE Id = {id} AND Deleted = 0;"; 
//                TaskItem TaskItem = dbConnection.QuerySingle<TaskItem>(query);               

//                return TaskItem;
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

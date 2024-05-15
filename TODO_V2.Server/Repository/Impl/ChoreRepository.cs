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

        public async Task<Chore> Add(Chore chore)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                try
                {
                    string query = @$"INSERT INTO Chores (CategoryID, UserID, State, TaskName, ExpirationDate) VALUES ('{chore.CategoryID}', '{chore.UserID}', '{chore.State}', '{chore.TaskName}', '{chore.ExpirationDate};";
                    dbConnection.Execute(query);
                }
                catch (Exception) { }
            }

            //var addedEntity = (await _context.AddAsync(chore)).Entity;
            //context.SaveChanges();
            //return addedEntity;

            return chore;
        }

        public async Task<Chore> Update(Chore chore)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @$"UPDATE Chores SET CategoryID = '{chore.CategoryID}', UserID = '{chore.UserID}', State = '{chore.State}', TaskName = '{chore.TaskName}', ExpirationDate = '{chore.ExpirationDate}';";

                dbConnection.Execute(query);
            }
            //var updatedEntity = _context.Update(entity).Entity;
            //await context.SaveChangesAsync();
            //return updatedEntity;
            return chore;
        }

        public void Delete(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = $"DELETE FROM Chores WHERE Id = {id};";

                dbConnection.Execute(query);
            }

            //var entity = _context.Find<T>(id);
            //if (entity != null) _context.Remove(entity);
            //context.SaveChanges();
        }

        public void LogicDelete(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = $"UPDATE Chores SET Deleted = 1 WHERE Id = {id};";

                dbConnection.Execute(query);
            }
            //var entity = _context.Find<T>(id);
            //if (entity != null) _context.Remove(entity);
            //context.SaveChanges();
        }

        public async Task<IEnumerable<Chore>> GetAll(GetRequest<Chore> request)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @"SELECT * FROM Chores;";

                return dbConnection.Query<Chore>(query);
            }
            //IQueryable<T> query = _context.Set<T>();

            //if (request.Filter != null)
            //{
            //    query = query.Where(request.Filter);
            //}

            //if (request.OrderBy != null)
            //{
            //    query = request.OrderBy(query);
            //}

            //if (request.Skip.HasValue)
            //{
            //    query = query.Skip(request.Skip.Value);
            //}

            //if (request.Take.HasValue)
            //{
            //    query = query.Take(request.Take.Value);
            //}

            //return query.ToList();
        }


        public async Task<IEnumerable<Chore>> GetAllLogic(GetRequest<Chore> request)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @"SELECT * FROM Chores WHERE Deleted = 0;";

                return dbConnection.Query<Chore>(query);
            }

            //IQueryable<T> query = _context.Set<T>();

            //if (request.Filter != null)
            //{
            //    query = query.Where(request.Filter);
            //}

            //if (request.OrderBy != null)
            //{
            //    query = request.OrderBy(query);
            //}

            //if (request.Skip.HasValue)
            //{
            //    query = query.Skip(request.Skip.Value);
            //}

            //if (request.Take.HasValue)
            //{
            //    query = query.Take(request.Take.Value);
            //}

            //return query.ToList();
        }

        public async Task<Chore?> GetById(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                //string query = $"SELECT * FROM Chores WHERE Id = {id} AND Deleted = 0;";
                string query = $"SELECT * FROM Chores WHERE Id = {id};";

                Chore chore = dbConnection.QuerySingle<Chore>(query);
                //return await context.FindAsync<T>(entityId);

                return chore;
            }
        }

    }
}

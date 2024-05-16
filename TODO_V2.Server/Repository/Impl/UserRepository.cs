using TODO_V2.Server.Repository.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using TODO_V2.Shared.Models;
using Microsoft.EntityFrameworkCore;
using static Dapper.SqlMapper;
using System.Linq;
using System.Collections.Generic;
using TODO_V2.Shared.Models.Enum;


namespace TODO_V2.Server.Repository.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private string ConnectionString => _configuration.GetConnectionString("TODO_V2DB");

        //private readonly UserContext context;

        public UserRepository(IConfiguration configuration)
        {
            //this.context = context;
            _configuration = configuration;
        }


        public async Task<User> Add(User user)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                try { 
                string query = @$"INSERT INTO Users (Name, Surname, UserName, Password, UserType) 
                                VALUES ('{user.Name}', '{user.Surname}', '{user.UserName}', '{user.Password}', '{user.UserType}');";               
                dbConnection.Execute(query);
                }
                catch (Exception) { }
            }

            //var addedEntity = (await _context.AddAsync(user)).Entity;
            //context.SaveChanges();
            //return addedEntity;

            return user;
        }

        public async Task<User> Update(User user)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @$"UPDATE Users SET Name = '{user.Name}', Surname ='{user.Surname}, Username = '{user.UserName}', Password ='{user.Password}', UserType = '{user.UserType}' WHERE Id = '{user.Id}';";

                dbConnection.Execute(query);
            }
            //var updatedEntity = _context.Update(entity).Entity;
            //await context.SaveChangesAsync();
            //return updatedEntity;
            return user;
        }

        public void Delete(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = $"DELETE FROM Users WHERE Id = {id};";

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
                string query = $"UPDATE Users SET Deleted = 1 WHERE Id = {id};";

                dbConnection.Execute(query);
            }
            //var entity = _context.Find<T>(id);
            //if (entity != null) _context.Remove(entity);
            //context.SaveChanges();
        }

        public  async Task<IEnumerable<User>> GetAll(GetRequest<User> request)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @"SELECT * FROM Users;";

                return dbConnection.Query<User>(query);
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


        public async Task<IEnumerable<User>> GetAllLogic(GetRequest<User> request)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @"SELECT * FROM Users WHERE Deleted = 0;";

                return dbConnection.Query<User>(query);
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

        public async Task<User?> GetById(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                //string query = $"SELECT * FROM Users WHERE Id = {id} AND Deleted = 0;";
                string query = $"SELECT * FROM Users WHERE Id = {id};";

                User user = dbConnection.QuerySingle<User>(query);
                //return await context.FindAsync<T>(entityId);

                return user;
            }
        }       
    }
}

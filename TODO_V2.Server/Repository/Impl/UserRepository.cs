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
using Microsoft.AspNetCore.Mvc;


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


        //public async Task<User> Add(User user)
        //{
        //    using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
        //    {
        //        string query = @$"INSERT INTO Users (Name, Surname, UserName, Password, UserType) 
        //                            VALUES ('{user.Name}', '{user.Surname}', '{user.UserName}', '{user.Password}', '{user.UserType}');";
        //        dbConnection.Execute(query);

        //        return user;
        //    }
        //}

        public async Task<bool> Add(User user)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @$"INSERT INTO Users (Name, Surname, UserName, Password, UserType) 
                                    VALUES ('{user.Name}', '{user.Surname}', '{user.UserName}', '{user.Password}', '{user.UserType}');";
                dbConnection.Execute(query);
            }
            return true;
        }

        public async Task<User> Update(User user)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @$"UPDATE Users SET Name = '{user.Name}', Surname ='{user.Surname}, Username = '{user.UserName}', Password ='{user.Password}', UserType = '{user.UserType}' WHERE Id = '{user.Id}';";

                dbConnection.Execute(query);
            }         
            return user;
        }

        public void Delete(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = $"DELETE FROM Users WHERE Id = {id};";

                dbConnection.Execute(query);
            }
        }

        public void LogicDelete(int id)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = $"UPDATE Users SET Deleted = 1 WHERE Id = {id};";

                dbConnection.Execute(query);
            }
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
                string query = $"SELECT * FROM Users WHERE Id = {id} AND Deleted = 0;";                

                User user = dbConnection.QuerySingle<User>(query);
                return user;
            }
        }

        public async Task<User?> GetByUserName(string username)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {           
                string query = $"SELECT * FROM Users WHERE UserName = '{username}' AND Deleted = 0;";

                User user = dbConnection.QuerySingle<User>(query);
                return user;
            }
        }

        public ActionResult<int> Count()
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = $"SELECT COUNT(Id) FROM Users";
                int count = dbConnection.QuerySingle<int>(query);

                return count;
            }
        }
    }
}

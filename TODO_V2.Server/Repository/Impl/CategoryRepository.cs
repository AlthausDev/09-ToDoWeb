using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using TODO_V2.Server.Repository.Interfaces;
using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Repository.Impl
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IConfiguration _configuration;
        private string ConnectionString => _configuration.GetConnectionString("TODO_V2DB");

        public CategoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> Add(Category category, object? secondEntity)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
                {
                    _ = await dbConnection.ExecuteAsync(@"
                        INSERT INTO Categories (Name) 
                        VALUES (@Name)", category);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar categoría: {ex.Message}");
                return false;
            }
        }

        public async Task<int> Count()
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                return await dbConnection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Categories");
            }
        }

        public async Task<bool> Delete(int entityId)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
                {
                    _ = await dbConnection.ExecuteAsync("DELETE FROM Categories WHERE Id = @Id", new { Id = entityId });
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar categoría: {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<Category>> GetAll(GetRequest<Category> request)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                return await dbConnection.QueryAsync<Category>("SELECT * FROM Categories");
            }
        }

        public async Task<IEnumerable<Category>> GetAllLogic(GetRequest<Category> request)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                return await dbConnection.QueryAsync<Category>("SELECT * FROM Categories WHERE IsActive = 1");
            }
        }

        public async Task<Category> GetById(int entityId)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                return await dbConnection.QueryFirstOrDefaultAsync<Category>("SELECT * FROM Categories WHERE Id = @Id", new { Id = entityId });
            }
        }

        public async Task<bool> LogicDelete(int entityId)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
                {
                    _ = await dbConnection.ExecuteAsync("UPDATE Categories SET IsActive = 0 WHERE Id = @Id", new { Id = entityId });
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar lógicamente categoría: {ex.Message}");
                return false;
            }
        }

        public async Task<Category> Update(Category category, object? secondEntity)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
                {
                    _ = await dbConnection.ExecuteAsync(@"
                        UPDATE Categories 
                        SET Name = @Name, UpdatedAt = GETDATE(), UpdatedBy = @UpdatedBy
                        WHERE Id = @Id", new
                    {
                        category.Name,
                        category.UpdatedBy,
                        category.Id
                    });
                }
                return category;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar categoría: {ex.Message}");
                return null;
            }
        }
    }
}

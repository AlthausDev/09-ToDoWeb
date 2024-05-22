using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TODO_V2.Server.Repository.Impl;
using TODO_V2.Server.Repository.Interfaces;
using TODO_V2.Server.Services.Interfaces;
using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository CategoryRepository;
        private readonly IConfiguration configuration;

        public CategoryService(ICategoryRepository categoryRepository, IConfiguration configuration)
        {
            this.configuration = configuration;
            CategoryRepository = categoryRepository;
        }

        public async Task<bool> Add(Category category, object? secondEntity)
        {
            try
            {
                return await CategoryRepository.Add(category, secondEntity);
            }
            catch (Exception ex)
            {                
                Console.WriteLine($"Error al agregar la categoría: {ex.Message}");
                throw;
            }
        }

        public async Task<int> Count()
        {
            try
            {
                return await CategoryRepository.Count();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error al contar las categorías: {ex.Message}");
                throw;
            }
        }

        public void Delete(int entityId)
        {
            try
            {
                CategoryRepository.Delete(entityId);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error al eliminar la categoría: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Category>> GetAll(GetRequest<Category> request)
        {
            try
            {
                return await CategoryRepository.GetAll(request);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error al obtener todas las categorías: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Category>> GetAllLogic(GetRequest<Category> request)
        {
            try
            {
                return await CategoryRepository.GetAllLogic(request);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error al obtener todas las categorías lógicas: {ex.Message}");
                throw;
            }
        }

        public async Task<Category> GetById(int entityId)
        {
            try
            {
                return await CategoryRepository.GetById(entityId);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error al obtener la categoría por Id: {ex.Message}");
                throw;
            }
        }

        public void LogicDelete(int entityId)
        {
            try
            {
                CategoryRepository.LogicDelete(entityId);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error al eliminar lógicamente la categoría: {ex.Message}");
                throw;
            }
        }

        public async Task<Category> Update(Category category, object? secondEntity)
        {
            try
            {
                return await CategoryRepository.Update(category, secondEntity);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error al actualizar la categoría: {ex.Message}");
                throw;
            }
        }
    }
}

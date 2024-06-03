using TODO_V2.Server.Repository.Interfaces;
using TODO_V2.Server.Services.Interfaces;
using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Services.Impl
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskRepository TaskRepository;
        private readonly IConfiguration configuration;

        public TaskItemService(ITaskRepository categoryRepository, IConfiguration configuration)
        {
            this.configuration = configuration;
            TaskRepository = categoryRepository;
        }


        public async Task<bool> Add(TaskItem entity, object? secondEntity)
        {
            try
            {
                return await TaskRepository.Add(entity, secondEntity);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al agregar la tarea: {ex.Message}");
                throw;
            }
        }

        public async Task<int> Count()
        {
            try
            {
                return await TaskRepository.Count();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al contar las tareas: {ex.Message}");
                throw;
            }
        }

        public void Delete(int entityId)
        {
            try
            {
                _ = TaskRepository.Delete(entityId);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al eliminar la tarea: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TaskItem>> GetAll(GetRequest<TaskItem> request)
        {
            try
            {
                return await TaskRepository.GetAll(request);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al obtener todas las tareas: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TaskItem>> GetAllLogic(GetRequest<TaskItem> request)
        {
            try
            {
                return await TaskRepository.GetAllLogic(request);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al obtener todas las tareas lógicas: {ex.Message}");
                throw;
            }
        }

        public async Task<TaskItem> GetById(int entityId)
        {
            try
            {
                return await TaskRepository.GetById(entityId);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al obtener la tarea por Id: {ex.Message}");
                throw;
            }
        }

        public async void LogicDelete(int entityId)
        {
            try
            {
                _ = await TaskRepository.LogicDelete(entityId);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al eliminar lógicamente la tarea: {ex.Message}");
                throw;
            }
        }

        public async Task<TaskItem> Update(TaskItem entity, object? secondEntity)
        {
            try
            {
                return await TaskRepository.Update(entity, secondEntity);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al actualizar la tarea: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByUserId(int userId)
        {
            return await TaskRepository.GetTasksByUserId(userId);
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByCategoryId(int categoryId)
        {
            try
            {
                return await TaskRepository.GetTasksByCategoryId(categoryId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener las tareas de la categoría: {ex.Message}", ex);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TODO_V2.Server.Repository.Interfaces;
using TODO_V2.Server.Services.Interfaces;
using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Services.Impl
{
    public class TaskItemService : IGenericService<TaskItem, object>
    {
        private readonly ITaskRepository _taskRepository;

        public TaskItemService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        }

        public async Task<bool> Add(TaskItem entity, object? secondEntity)
        {
            try
            {
                return await _taskRepository.Add(entity, secondEntity);
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
                return await _taskRepository.Count();
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
                _taskRepository.Delete(entityId);
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
                return await _taskRepository.GetAll(request);
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
                return await _taskRepository.GetAllLogic(request);
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
                return await _taskRepository.GetById(entityId);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al obtener la tarea por Id: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> LogicDelete(int entityId)
        {
            try
            {
                return await _taskRepository.LogicDelete(entityId);
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
                return await _taskRepository.Update(entity, secondEntity);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al actualizar la tarea: {ex.Message}");
                throw;
            }
        }

        void IGenericService<TaskItem, object>.LogicDelete(int entityId)
        {
            throw new NotImplementedException();
        }
    }
}

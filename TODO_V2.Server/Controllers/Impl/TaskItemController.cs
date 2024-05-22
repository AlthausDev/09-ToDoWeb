//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using TODO_V2.Server.Services.Interfaces;
//using TODO_V2.Shared;
//using TODO_V2.Shared.Models;

//namespace TODO_V2.Server.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class TaskItemController : ControllerBase
//    {
//        private readonly IGenericService<TaskItem, object> _taskService;

//        public TaskItemController(IGenericService<TaskItem, object> taskService)
//        {
//            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
//        }

//        [HttpPost]
//        public async Task<IActionResult> CreateTask(TaskItem taskItem)
//        {
//            try
//            {
//                bool result = await _taskService.Add(taskItem, null);
//                return Ok(result);
//            }
//            catch (Exception ex)
//            {
//
//                Console.WriteLine($"Error al crear la tarea: {ex.Message}");
//                return StatusCode(500, "Error interno del servidor");
//            }
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAllTasks()
//        {
//            try
//            {
//                var tasks = await _taskService.GetAll(new GetRequest<TaskItem>());
//                return Ok(tasks);
//            }
//            catch (Exception ex)
//            {
//
//                Console.WriteLine($"Error al obtener todas las tareas: {ex.Message}");
//                return StatusCode(500, "Error interno del servidor");
//            }
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetTaskById(int id)
//        {
//            try
//            {
//                var task = await _taskService.GetById(id);
//                if (task == null)
//                {
//                    return NotFound();
//                }
//                return Ok(task);
//            }
//            catch (Exception ex)
//            {
//
//                Console.WriteLine($"Error al obtener la tarea por Id: {ex.Message}");
//                return StatusCode(500, "Error interno del servidor");
//            }
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateTask(int id, TaskItem taskItem)
//        {
//            try
//            {
//                if (id != taskItem.Id)
//                {
//                    return BadRequest("El Id de la tarea no coincide con el Id proporcionado en la URL");
//                }

//                var updatedTask = await _taskService.Update(taskItem, null);
//                return Ok(updatedTask);
//            }
//            catch (Exception ex)
//            {
//
//                Console.WriteLine($"Error al actualizar la tarea: {ex.Message}");
//                return StatusCode(500, "Error interno del servidor");
//            }
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteTask(int id)
//        {
//            try
//            {
//                await _taskService.Delete(id);
//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//
//                Console.WriteLine($"Error al eliminar la tarea: {ex.Message}");
//                return StatusCode(500, "Error interno del servidor");
//            }
//        }
//    }
//}

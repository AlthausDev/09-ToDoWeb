using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TODO_V2.Server.Services.Interfaces;
using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Controllers.Impl
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskItemController : ControllerBase
    {
        private readonly ILogger<TaskItemController> _logger;
        private readonly ITaskItemService _TaskItemService;

        public TaskItemController(ILogger<TaskItemController> logger, ITaskItemService TaskItemService)
        {
            _logger = logger;
            _TaskItemService = TaskItemService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(TaskItem taskItem)
        {
            try
            {
                bool result = await _TaskItemService.Add(taskItem, null);
                return Ok(result);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al crear la tarea: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var tasks = await _TaskItemService.GetAll(new GetRequest<TaskItem>());
                return Ok(tasks);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al obtener todas las tareas: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                var task = await _TaskItemService.GetById(id);
                return task == null ? NotFound() : Ok(task);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al obtener la tarea por Id: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskItem taskItem)
        {
            try
            {
                if (id != taskItem.Id)
                {
                    return BadRequest("El Id de la tarea no coincide con el Id proporcionado en la URL");
                }

                var updatedTask = await _TaskItemService.Update(taskItem, null);
                return Ok(updatedTask);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al actualizar la tarea: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                _TaskItemService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al eliminar la tarea: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [AllowAnonymous]
        [HttpGet("count")]
        public Task<int> Count()
        {
            return _TaskItemService.Count();
        }

        [HttpGet("user/{userId}/tasks")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasksByUserId(int userId)
        {
            var tasks = await _TaskItemService.GetTasksByUserId(userId);
            return Ok(tasks);
        }

        [HttpGet("category/{categoryId}/tasks")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasksByCategoryId(int categoryId)
        {
            try
            {
                var tasks = await _TaskItemService.GetTasksByCategoryId(categoryId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las tareas de la categoría: {ex.Message}");
                return StatusCode(500, "Error interno del servidor al obtener las tareas de la categoría.");
            }
        }


    }
}

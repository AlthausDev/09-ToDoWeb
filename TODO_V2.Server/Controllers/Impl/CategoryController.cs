using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TODO_V2.Server.Controllers.Impl;
using TODO_V2.Server.Services.Impl;
using TODO_V2.Server.Services.Interfaces;
using TODO_V2.Shared;
using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

       
        [HttpPost]
        public async Task<IActionResult> Post(Category category)
        {
            try
            {
                bool result = await _categoryService.Add(category, null);
                return Ok(result);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al crear la categoría: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _categoryService.GetAll(new GetRequest<Category>());
                return Ok(categories);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al obtener todas las categorías: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetById(id);
                if (category == null)
                {
                    return NotFound();
                }
                return Ok(category);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al obtener la categoría por Id: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            try
            {
                if (id != category.Id)
                {
                    return BadRequest("El Id de la categoría no coincide con el Id proporcionado en la URL");
                }

                var updatedCategory = await _categoryService.Update(category, null);
                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al actualizar la categoría: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [AllowAnonymous]
        [HttpGet("count")]
        public Task<int> Count()
        {
            return _categoryService.Count();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                _categoryService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al eliminar la categoría: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}

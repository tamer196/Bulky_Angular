using BulkyBook.DataAccess.Repositiry.IRepository;
using BulkyBook.Models;
using BulkyBooks.DataAcces.Data;
using BulkyBooks.DataAccess.Repositiry.IRepository;
using BulkyBooks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyBooksWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet(Name = "GetCategories")]
        public IActionResult Get()
        {
            try
            {
                var categories = _unitOfWork.category.GetAll().ToList();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve categories. " + ex.Message);
            }
        }

        [HttpPost("Create", Name = "CreateCategory")]
        public IActionResult Create([FromBody] Category category)
        {
            try
            {
                if (category == null || string.IsNullOrEmpty(category.Name) || category.DisplayOrder <= 0)
                    return BadRequest("Invalid category data.");

                _unitOfWork.category.Add(category);
                _unitOfWork.Save();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create category. " + ex.Message);
            }
        }

        [HttpPost("Update", Name = "UpdateCategory")]
        public IActionResult Update([FromBody] Category category)
        {
            try
            {
                if (category == null || string.IsNullOrEmpty(category.Name) || category.DisplayOrder <= 0)
                    return BadRequest("Invalid category data.");

                // Validate if the ID is not zero
                if (category.Id == 0)
                {
                    return BadRequest("Product ID cannot be zero.");
                }

                // Check if the product exists
                var existingCategory = _unitOfWork.category.Get(cat => cat.Id == category.Id);
                if (existingCategory == null)
                {
                    return NotFound("Category not found.");
                }

                _unitOfWork.Detach(existingCategory);
                _unitOfWork.category.Update(category);
                _unitOfWork.Save();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update category. " + ex.Message);
            }
        }


        [HttpDelete("Delete", Name = "DeleteCategory")]
        public IActionResult Delete(int id)
        {
            try
            {
                var category = _unitOfWork.category.Get(cat => cat.Id == id);

                if (category == null)
                    return NotFound("Category not found.");

                _unitOfWork.category.Remove(category);
                _unitOfWork.Save();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete category. " + ex.Message);
            }
        }

    }
}

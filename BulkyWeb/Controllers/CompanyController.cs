using BulkyBook.DataAccess.Repositiry.IRepository;
using BulkyBook.Models;
using BulkyBooks.DataAcces.Data;
using BulkyBooks.DataAccess.Repositiry.IRepository;
using BulkyBooks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyBooksWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet(Name = "GetCompanies")]
        public IActionResult Get()
        {
            try
            {
                var companies = _unitOfWork.company.GetAll().ToList();
                return Ok(companies);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve categories. " + ex.Message);
            }
        }

        [HttpPost("Create", Name = "CreateCompany")]
        public IActionResult Create([FromBody] Company company)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid Company data.");

                _unitOfWork.company.Add(company);
                _unitOfWork.Save();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create Company. " + ex.Message);
            }
        }

        [HttpPost("Update", Name = "UpdateCompany")]
        public IActionResult Update([FromBody] Company company)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid Company data.");

                // Validate if the ID is not zero
                if (company.Id == 0)
                {
                    return BadRequest("Company ID cannot be zero.");
                }

                // Check if the product exists
                var existingCompany = _unitOfWork.company.Get(com => com.Id == company.Id);
                if (existingCompany == null)
                {
                    return NotFound("Company not found.");
                }

                _unitOfWork.Detach(existingCompany);
                _unitOfWork.company.Update(company);
                _unitOfWork.Save();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update Company. " + ex.Message);
            }
        }


        [HttpDelete("Delete", Name = "DeleteCompany")]
        public IActionResult Delete(int id)
        {
            try
            {
                var company = _unitOfWork.company.Get(com => com.Id == id);

                if (company == null)
                    return NotFound("Company not found.");

                _unitOfWork.company.Remove(company);
                _unitOfWork.Save();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete Company. " + ex.Message);
            }
        }

    }
}

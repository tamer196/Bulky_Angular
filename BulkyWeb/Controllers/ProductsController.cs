using BulkyBook.DataAccess.Repositiry.IRepository;
using BulkyBook.Models;
using BulkyBooks.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet(Name = "GetProducts")]
        public IActionResult Get()
        {
            try
            {
                var products = _unitOfWork.product.GetAll(includeProperites : "Category").ToList();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve products. " + ex.Message);
            }
        }

        [HttpPost("Create", Name = "CreateProduct")]
        public IActionResult Create([FromForm] Product product,IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

               
                if (product.ListPrice <= 0 || product.Price <= 0 || product.Price50 <= 0 || product.Price100 <= 0)
                {
                    return BadRequest("Prices must be greater than zero.");
                }


                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string angularProductPath = @"D:\tamer\bulky_front\Bulky_Angular_Front\bulky\src\assets\images\products";

                    using(FileStream angularFileStream = new FileStream(Path.Combine(angularProductPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(angularFileStream);
                    }

                    product.ImageUrl = "assets/images/products/" + fileName;
                }

                _unitOfWork.product.Add(product);
                _unitOfWork.Save();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create product. " + ex.Message);
            }
        }


        [HttpPost("Update", Name = "UpdateProduct")]
        public IActionResult Update([FromForm] Product product, IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (product.Id == 0)
                {
                    return BadRequest("Product ID cannot be zero.");
                }

                var existingProduct = _unitOfWork.product.Get(prod => prod.Id == product.Id);
                if (existingProduct == null)
                {
                    return NotFound("Product not found.");
                }

                if (product.ListPrice <= 0 || product.Price <= 0 || product.Price50 <= 0 || product.Price100 <= 0)
                {
                    return BadRequest("Prices must be greater than zero.");
                }

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string oldAngularProductPath = @"D:\tamer\bulky_front\Bulky_Angular_Front\bulky\src\";

                    if (!string.IsNullOrEmpty(existingProduct.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(oldAngularProductPath, existingProduct.ImageUrl.TrimStart('\\'));

                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                    }

                    string angularProductPath = @"D:\tamer\bulky_front\Bulky_Angular_Front\bulky\src\assets\images\products";

                    using (FileStream angularFileStream = new FileStream(Path.Combine(angularProductPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(angularFileStream);
                    }

                    product.ImageUrl = "assets/images/products/" + fileName;
                }

                _unitOfWork.Detach(existingProduct);

                _unitOfWork.product.Update(product);
                _unitOfWork.Save();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update product. " + ex.Message);
            }
        }



        [HttpDelete("Delete", Name = "DeleteProduct")]
        public IActionResult Delete(int id)
        {
            try
            {
                var product = _unitOfWork.product.Get(prod => prod.Id == id);

                if (product == null)
                    return NotFound("Product not found.");

                string angularProductPath = @"D:\tamer\bulky_front\Bulky_Angular_Front\bulky\src\";

                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var oldImagePath = Path.Combine(angularProductPath, product.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                }


                _unitOfWork.product.Remove(product);
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

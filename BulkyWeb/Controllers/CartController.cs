using BulkyBook.DataAccess.Repositiry.IRepository;
using BulkyBooks.Models;
using BulkyBooks.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpPost("Add", Name = "AddToCart")]
        public async Task<IActionResult> AddToCart(ShoppingCart shoppingCart)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst("User ID").Value;
                shoppingCart.ApplicationUserId = userId;

                ShoppingCart cartFromDb = _unitOfWork.shoppingCart.Get(u => u.ApplicationUserId == userId &&
                u.ProductId == shoppingCart.ProductId);

                if (cartFromDb != null)
                {
                    //shopping cart exists
                    cartFromDb.Count += shoppingCart.Count;
                    _unitOfWork.shoppingCart.Update(cartFromDb);
                    _unitOfWork.Save();
                }
                else
                {
                    //add cart record
                    _unitOfWork.shoppingCart.Add(shoppingCart);
                    _unitOfWork.Save();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create product. " + ex.Message);
            }
        }



        [HttpGet("GetCartOrders", Name = "GetCartOrders")]
        public IActionResult GetCartOrders()
        {
            try
            {
                var products = _unitOfWork.product.GetAll(includeProperties: "Category").ToList();
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst("User ID").Value;
                var shoppingCartVM = new ShoppingCartVM()
                {
                    ShoppingCartList = _unitOfWork.shoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product").ToList()
                };

                foreach (var cart in shoppingCartVM.ShoppingCartList)
                {
                    cart.Price = GetPriceBasedOnQuantity(cart);
                    shoppingCartVM.OrderTotal += (cart.Price * cart.Count);
                }

                return Ok(shoppingCartVM);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve products. " + ex.Message);
            }
        }


        [HttpDelete("Pluse", Name = "Pluse")]
        public IActionResult Pluse(int id)
        {
            try
            {
                var cart = _unitOfWork.shoppingCart.Get(u => u.Id == id);
                if (cart != null)
                {
                    cart.Count += 1;
                    _unitOfWork.shoppingCart.Update(cart);
                    _unitOfWork.Save();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Increase Item. " + ex.Message);
            }
        }

        [HttpDelete("Minus", Name = "Minus")]
        public IActionResult Minus(int id)
        {
            try
            {
                var cart = _unitOfWork.shoppingCart.Get(u => u.Id == id);
                if (cart != null)
                {
                    if (cart.Count <= 1)
                    {
                        _unitOfWork.shoppingCart.Remove(cart);
                    }
                    else
                    {
                        cart.Count -= 1;
                        _unitOfWork.shoppingCart.Update(cart);
                    }
                    _unitOfWork.Save();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Decrease Item. " + ex.Message);
            }
        }

        [HttpDelete("Delete", Name = "Delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                var cart = _unitOfWork.shoppingCart.Get(u => u.Id == id);
                if (cart != null)
                {
                    _unitOfWork.shoppingCart.Remove(cart);
                    _unitOfWork.Save();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Delete Item. " + ex.Message);
            }
        }


        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                if (shoppingCart.Count <= 100)
                {
                    return shoppingCart.Product.Price50;
                }
                else
                {
                    return shoppingCart.Product.Price100;
                }
            }
        }
    }
}

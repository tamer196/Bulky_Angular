using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }


        [HttpGet(Name = "GetCategories")]
        public IEnumerable<Category> Get()
        {
            return _db.Category.ToList();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class ProductVM
    {
        [Required]
        public Product Product { get; set; }
        [ValidateNever]
        public IFormFile File { get; set; }
    }
}

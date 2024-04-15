using BulkyBooks.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class Register
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }        
        [Required]
        public string Role { get; set; }
        [Required]
        public string Name { get; set; }
        [ValidateNever]
        public string? StreetAddress { get; set; }
        [ValidateNever]
        public string? City { get; set; }
        [ValidateNever]
        public string? State { get; set; }
        [ValidateNever]
        public string? PostalCode { get; set; }
        [ValidateNever]
        public string? PhoneNumber { get; set; }
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [ValidateNever]
        public Company Company { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WallProj.Models
{
    public class UserLogModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    public class UserRegModel
    {
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        [Display(Name="First Name")]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        [Display(Name="Last Name")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Description { get; set; }
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Password and confirmation must match.")]
        [Display(Name="Confirm Password")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
    public class UserEditModel
    {
        [Required]
        [MinLength(2, ErrorMessage = "First Name must be 2 or more characters")]
        [MaxLength(30, ErrorMessage = "First Name must be 30 or fewer characters")]
        [Display(Name="First Name")]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Last Name must be 2 or more characters")]
        [MaxLength(30, ErrorMessage = "Last Name must be 30 or fewer characters")]
        [Display(Name="Last Name")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Description { get; set; }
    }
    public class AdminEditModel
    {
        public int UserId { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "First Name must be 2 or more characters")]
        [MaxLength(30, ErrorMessage = "First Name must be 30 or fewer characters")]
        [Display(Name="First Name")]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Last Name must be 2 or more characters")]
        [MaxLength(30, ErrorMessage = "Last Name must be 30 or fewer characters")]
        [Display(Name="Last Name")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Description { get; set; }
        [Required]
        public int PermissionLevel { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WallProj.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string LastName { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public int PermissionLevel { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
        [InverseProperty("Commenter")]
        public List<Comment> PostedComments { get; set; }
        [InverseProperty("Recipient")]
        public List<Comment> ReceivedComments { get; set; }
        public User()
        {
            PostedComments = new List<Comment>();
            ReceivedComments = new List<Comment>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WallProj.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        [ForeignKey("User")]
        public int CommenterUserId { get; set; }
        public User Commenter { get; set; }
        
        [ForeignKey("User")]
        public int RecipientUserId { get; set; }
        public User Recipient { get; set; }
        public string Text { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
        [InverseProperty("Parent")]
        public List<Reply> Replies { get; set; }
        public Comment()
        {
            Replies = new List<Reply>();
        }
    }
}
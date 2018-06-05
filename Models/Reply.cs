using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WallProj.Models
{
    public class Reply
    {
        [Key]
        public int ReplyId { get; set; }
        [ForeignKey("User")]
        public int PosterUserId { get; set; }
        public User Poster { get; set; }
        public int ParentCommentId { get; set; }
        public Comment Parent { get; set; }
        public string Text { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
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
    }
}
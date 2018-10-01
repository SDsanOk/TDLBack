using System;
using System.Collections.Generic;
using System.Text;
using Dapper;

namespace CloudCall.Todo.DAL.Models
{
    public class Board
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
    }
}

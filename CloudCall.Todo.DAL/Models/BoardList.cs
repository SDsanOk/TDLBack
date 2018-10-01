using System;
using System.Collections.Generic;
using System.Text;
using Dapper;

namespace CloudCall.Todo.DAL.Models
{
    public class BoardList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int BoardId { get; set; }
        [Required]
        public int ListId { get; set; }
    }
}

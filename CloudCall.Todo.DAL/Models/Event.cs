using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCall.Todo.DAL
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public int ListId { get; set; }
        [Required]
        public string Title { get; set; }
        public bool Done { get; set; } = false;
    }
}

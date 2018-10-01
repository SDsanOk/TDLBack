using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace CloudCall.Todo.DAL
{
    public class ListEvent
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ListId { get; set; }
        [Required]
        public int EventId { get; set; }
    }
}

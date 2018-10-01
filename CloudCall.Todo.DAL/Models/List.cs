using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace CloudCall.Todo.DAL
{
    public class List
    {
        public List()
        {
            Todo = new List<Event>();
        }

        [Key]
        public int Id { get; set; }
        public  int BoardId { get; set; }
        [Required]
        public string Title { get; set; }
        public  List<Event> Todo { get; set; }
    }
}

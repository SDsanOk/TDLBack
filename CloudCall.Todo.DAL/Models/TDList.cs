using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace CloudCall.Todo.DAL
{
    public class TDList
    {
        public TDList()
        {
            Events = new List<TDEvent>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public  List<TDEvent> Events { get; set; }
    }
}

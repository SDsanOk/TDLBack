using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace CloudCall.Todo.DAL
{
    public class ApplicationUserTDList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ApplicationUserId { get; set; }
        [Required]
        public int TDListId { get; set; }
    }
}

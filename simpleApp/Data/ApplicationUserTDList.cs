using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace simpleApp.Data
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

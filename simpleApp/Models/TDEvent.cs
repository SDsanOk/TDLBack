using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using System.Threading.Tasks;

namespace simpleApp.Models
{
    public class TDEvent
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public bool Done { get; set; } = false;
    }
}

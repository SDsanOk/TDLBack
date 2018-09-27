using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace simpleApp.Data
{
    public class TDListTDEvent
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int TDListId { get; set; }
        [Required]
        public int TDEventId { get; set; }
    }
}

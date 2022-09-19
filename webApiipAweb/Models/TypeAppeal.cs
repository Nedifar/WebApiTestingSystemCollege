using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class TypeAppeal
    {
        [Key]
        public int idTypeAppeal { get; set; }
        public string typeName { get; set; }
        public virtual List<Appeal> Appeals { get; set; } = new List<Appeal>();
    }
}

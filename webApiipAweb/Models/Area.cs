using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webApiipAweb.Models
{
    public class Area
    {
        [Key]
        public int idArea { get; set; }

        public string areaName { get; set; }

        [ForeignKey("Region")]
        public int idRegion { get; set; }

        public virtual Region Region { get; set; }

        public virtual List<Municipality> Municipalities { get; set; } = new List<Municipality>(); 
    }
}

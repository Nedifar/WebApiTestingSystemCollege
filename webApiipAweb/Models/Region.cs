using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace webApiipAweb.Models
{
    public class Region
    {
        [Key]
        public int idRegion { get; set; }

        public string regionName { get; set; }

        public virtual List<Area> Areas { get; set; } = new List<Area>();
    }
}

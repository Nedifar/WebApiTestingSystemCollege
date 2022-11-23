using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class Thing
    {
        [Key]
        public int idThing { get; set; }
        [ForeignKey("ThingPack")]
        public int idthingPack { get; set; }
        [JsonIgnore]
        public virtual ThingPack ThingPack { get; set; }
        public double xPosition { get; set; }
        public double yPosition { get; set; }
        public string urlImage { get; set; }
        public int price { get; set; }
        [JsonIgnore]
        public virtual List<ThingExecution> ThingExecutions { get; set; } = new List<ThingExecution>();
    }
}

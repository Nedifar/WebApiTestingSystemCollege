using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class ThingExecution
    {
        [Key]
        public int idThingExecution { get; set; }
        [ForeignKey("Thing")]
        public int idThing { get; set; }
        public virtual Thing Thing { get; set; }
        public bool isFinished { get; set; }
        [JsonIgnore]
        public virtual List<ThingPackExecution> ThingPackExecutions { get; set; } = new List<ThingPackExecution>();
    }
}

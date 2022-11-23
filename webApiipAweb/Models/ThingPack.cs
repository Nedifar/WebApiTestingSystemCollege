using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class ThingPack
    {
        [Key]
        public int idThingPack { get; set; }
        public virtual List<Thing> Things { get; set; } = new List<Thing>();
        public string namePack { get; set; }
        public string imageOfPack { get; set; }
        public virtual List<Subject> Subjects { get; set; } = new List<Subject>();
        [JsonIgnore]
        public virtual List<ThingPackExecution> ThingPackExecutions { get; set; } = new List<ThingPackExecution>();
    }
}

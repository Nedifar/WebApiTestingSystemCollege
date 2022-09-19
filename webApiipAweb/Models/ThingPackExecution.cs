using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class ThingPackExecution
    {
        [Key]
        public int idThingPackExecution { get; set; }
        [ForeignKey("Child")]
        public string ChildId { get; set; }
        [JsonIgnore]
        public virtual Child Child { get; set; }
        [ForeignKey("ThingPack")]
        public int idThingPack { get; set; }
        public virtual ThingPack ThingPack { get; set; }
        public bool isCompleted { get; set; }
        public virtual List<ThingExecution> ThingExecutions { get; set; } = new List<ThingExecution>();
        public int packCompletedProcent
        {
            get
            {
                try
                {
                    return 100 * ThingExecutions.Where(p => p.isFinished).Count() / ThingExecutions.Count();
                }
                catch { return 0; }
            }
        }
    }
}

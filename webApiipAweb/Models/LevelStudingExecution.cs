using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class LevelStudingExecution
    {
        [Key]
        public int idLevelStudingExecution { get; set; }
        [ForeignKey("Child")]
        public string ChildId { get; set; }
        public virtual Child Child { get; set; }
        [ForeignKey("LevelStuding")]
        public int idLevelStuding { get; set; }
        public virtual LevelStuding LevelStuding { get; set; }
        public virtual List<SubjectExecution> SubjectExecutions { get; set; } = new List<SubjectExecution>();
    }
}

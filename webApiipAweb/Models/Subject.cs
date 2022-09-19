using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class Subject
    {
        [Key]
        public int idSubject { get; set; }
        public string nameSubject { get; set; }
        public virtual List<ThingPack> ThingPacks { get; set; } = new List<ThingPack>();
        [ForeignKey("LevelStuding")]
        public int idLevelStuding { get; set; }
        public virtual LevelStuding LevelStuding { get; set; }
        public virtual List<SubjectExecution> SubjectExecutions { get; set; } = new List<SubjectExecution>();
        public virtual List<Chapter> Chapters { get; set; } = new List<Chapter>();
    }
}

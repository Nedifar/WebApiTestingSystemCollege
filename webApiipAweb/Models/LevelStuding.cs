using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class LevelStuding
    {
        [Key]
        public int idLevelStuding { get; set; }
        public string nameLevel { get; set; }
        public virtual List<Subject> Subjects { get; set; } = new List<Subject>();
    }
}

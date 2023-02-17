using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class Achivment
    {
        [Key]
        public string idAchivment { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Term { get; set; }
        public virtual List<AchivmentExecution> AchivmentExecutions { get; set; } = new List<AchivmentExecution>();
    }
}
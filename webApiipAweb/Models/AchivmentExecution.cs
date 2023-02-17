using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class AchivmentExecution
    {
        [Key]
        public string idAchivmentExecution { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey("Achivment")]
        public string idAchivment { get; set; }
        
        public virtual Achivment Achivment { get; set; }
        [ForeignKey("Child")]
        public string ChildId { get; set; }
        public virtual Child Child { get; set; }
    }
}

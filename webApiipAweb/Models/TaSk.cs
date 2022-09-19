using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class TaskWithOpenAnsw
    {
        [Key]
        public int idTaskWithOpenAnsw { get; set; }
        public string textQuestion { get; set; }
        public string answear { get; set; }
        public string theme { get; set; }
        [ForeignKey("TestPack")]
        public int? idTestPack { get; set; }
        [JsonIgnore]
        public virtual TestPack TestPack { get; set; }
        public virtual List<Solution> Solutions { get; set; } = new List<Solution>();
    }
}

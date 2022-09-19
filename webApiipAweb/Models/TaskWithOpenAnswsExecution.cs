using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class TaskWithOpenAnswsExecution
    {
        [Key]
        public int idTaskWithOpenAnswsExecution { get; set; }
        [ForeignKey("TestPackExecution")]
        public int? idTestPackExecution { get; set; }
        [JsonIgnore]
        public virtual TestPackExecution TestPackExecution { get; set; }
        [ForeignKey("TaskWithOpenAnsws")]
        public int? idTaskWithOpenAnsws { get; set; }
        public virtual TaskWithOpenAnsw TaskWithOpenAnsws { get; set; }
        public string status { get; set; }
        public int timeExecutionInSecond { get; set; }
    }
}

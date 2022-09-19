using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class TryingTestTask
    {
        [Key]
        public int idTryingTestTask { get; set; }
        public int result { get; set; }
        public string status { get; set; }
        [ForeignKey("TestPackExecution")]
        public int idTestPackExecution { get; set; }
        public virtual TestPackExecution TestPackExecution { get; set; }
        public virtual List<TestTaskExecution> TestTaskExecutions { get; set; } = new List<TestTaskExecution>();
        public int timeExecutionInSecond { get; set; }
    }
}

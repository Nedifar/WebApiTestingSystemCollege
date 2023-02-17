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
        public string idTryingTestTask { get; set; } = Guid.NewGuid().ToString();

        public int result { get; set; }

        public string status { get; set; }

        public DateTime startDate { get; set; } = DateTime.Now;

        [ForeignKey("TestPackExecution")]
        public string idTestPackExecution { get; set; }
        public virtual TestPackExecution TestPackExecution { get; set; }

        public virtual List<TestTaskExecution> TestTaskExecutions { get; set; } = new List<TestTaskExecution>();

        public int timeExecutionInSecond { get; set; }
    }
}

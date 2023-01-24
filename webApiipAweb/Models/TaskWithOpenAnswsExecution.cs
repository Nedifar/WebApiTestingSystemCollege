using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class TaskExecutionParent
    {
        [Key]
        public int idTaskExecution { get; set; }

        [ForeignKey("TestPackExecution")]
        public int? idTestPackExecution { get; set; }

        [JsonIgnore]
        public virtual TestPackExecution TestPackExecution { get; set; }

        [NotMapped]
        public bool isHard { get; set; } = false;

        public double mark { get; set; }

        public StatusTaskExecution Status { get; set; }

        [NotMapped]
        public string themes { get; set; }

        public int timeExecutionInSecond { get; set; }

        public DateTime lockedTime { get; set; } = new DateTime(1970, 1, 1);

        public string GetStatus() => this.Status switch
        {
            StatusTaskExecution.AwaitingExecution => "Ожидает выполнения",
            StatusTaskExecution.InCorrect => "Решено неверно",
            StatusTaskExecution.PartCorrect => "Решено частично верно",
            StatusTaskExecution.Correct => "Решено верно",
            _ => throw new Exception()
        };
    }

    public class TaskWithOpenAnswsExecution : TaskExecutionParent
    {
        public string AnswearResult { get; set; }

        [ForeignKey("TaskWithOpenAnsw")]
        public int? idTask { get; set; }

        public virtual TaskWithOpenAnsw TaskWithOpenAnsw { get; set; }
    }

    public class TaskWithClosedAnswsExecution : TaskExecutionParent
    {
        [ForeignKey("AnswearOnTask")]
        public int? idAnswearOnTask { get; set; }

        public virtual AnswearOnTask AnswearOnTask { get; set; }

        [ForeignKey("TaskWithClosedAnsw")]
        public int? idTask { get; set; }

        public virtual TaskWithClosedAnsw TaskWithClosedAnsw { get; set; }
    }

    public enum StatusTaskExecution
    {
        AwaitingExecution,
        Correct,
        InCorrect,
        PartCorrect
    }
}

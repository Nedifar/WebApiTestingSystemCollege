using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class TestTaskExecution
    {
        [Key]
        public string idTestTaskExecution { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey("TestTask")]
        public string idTestTask { get; set; }
        public virtual TestTask TestTask { get; set; }

        [ForeignKey("TryingTestTask")]
        public string idTryingTestTask { get; set; }
        public virtual TryingTestTask TryingTestTask { get; set; }

        [ForeignKey("AnswearOnTask")]
        public string idAnswearOnTask { get; set; }
        public virtual AnswearOnTask AnswearOnTask { get; set; }

        public StatusExecution StatusExecution { get; set; } = StatusExecution.Default;
        public string GetStatus() => this.StatusExecution switch
        {
            StatusExecution.Correct => "Верно",
            StatusExecution.InCorrect => "Неверно",
            StatusExecution.Default => "Нет ответа",
            _ => throw new NotImplementedException()
        };
    }

    public enum StatusExecution
    {
        Correct,
        InCorrect,
        Default
    }
}

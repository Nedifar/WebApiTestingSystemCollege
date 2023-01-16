using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webApiipAweb.Models
{
    public class SessionProgress
    {
        [Key]
        public int idSessionProgress { get; set; }

        public int taskNumber { get; set; }

        [ForeignKey("SessionChapterExecution")]
        public int idSessionChapterExecution { get; set; }

        public virtual SessionChapterExecution SessionChapterExecution { get; set; }

        public StatusTaskExecution StatusTaskExecution { get; set; }

        public string GetStatus() => this.StatusTaskExecution switch
        {
            StatusTaskExecution.AwaitingExecution => "Ожидает выполнения",
            StatusTaskExecution.InCorrect => "Решено неверно",
            StatusTaskExecution.PartCorrect => "Решено частично верно",
            StatusTaskExecution.Correct => "Решено верно",
            _ => throw new Exception()
        };
    }

}

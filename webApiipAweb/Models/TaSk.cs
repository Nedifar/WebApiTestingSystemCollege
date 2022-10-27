using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class TaskWithOpenAnsw : ParentTask
    {
        public TaskWithOpenAnsw()
        {
            TypesTask = TypesTask.Opened;
        }
        public string answear { get; set; }
    }

    public class TaskWithClosedAnsw : ParentTask
    {
        public TaskWithClosedAnsw()
        {
            TypesTask = TypesTask.Closed;
        }
        public virtual List<AnswearOnTask> AnswearOnTask { get; set; } = new List<AnswearOnTask>();
    }
}

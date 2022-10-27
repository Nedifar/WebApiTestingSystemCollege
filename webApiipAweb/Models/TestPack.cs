using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class TestPack
    {
        [Key]
        public int idTestPack { get; set; }
        public string header { get; set; }
        [ForeignKey("Chapter")]
        public int idChapter { get; set; }
        public virtual Chapter Chapter { get; set; }
        public virtual List<TestTask> TestTasks { get; set; } = new List<TestTask>();
        public virtual List<TaskWithOpenAnsw> TaskWithOpenAnsws { get; set; } = new List<TaskWithOpenAnsw>();
        public virtual List<TaskWithClosedAnsw> TaskWithClosedAnsws { get; set; } = new List<TaskWithClosedAnsw>();
        public virtual List<TestPackExecution> TestPackExecutions { get; set; } = new List<TestPackExecution>();
    }
}

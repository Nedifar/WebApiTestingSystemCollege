using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class TestPackExecution
    {
        [Key]
        public int idTestPackExecution { get; set; }

        [ForeignKey("ChapterExecution")]
        public int idChapterExecution { get; set; }

        public virtual ChapterExecution ChapterExecution { get; set; }

        [ForeignKey("TestPack")]
        public int idTestPack { get; set; }

        public virtual TestPack TestPack { get; set; }

        public virtual List<TryingTestTask> TryingTestTasks { get; set; } = new List<TryingTestTask>();

        public int accessProcentFinalTest
        {
            get
            {
                return getProcentDecideTaskWithOpen;
            }
        }
        public bool haveFinalTest
        {
            get
            {
                if(TestPack.TestTasks.Count() !=0)
                {
                    return true;
                }
                return false;
            }
        }
        public virtual List<TaskWithOpenAnswsExecution> TaskWithOpenAnswsExecutions { get; set; } = new List<TaskWithOpenAnswsExecution>();

        public virtual List<TaskWithClosedAnswsExecution> TaskWithClosedAnswsExecutions{ get; set; } = new List<TaskWithClosedAnswsExecution>();

        public int getMaxProcentTest
        {
            get
            {
                if (TryingTestTasks.Count == 0)
                {
                    return 0;
                }
                return 100 * TryingTestTasks.Max(p => p.result) / TestPack.TestTasks.Count();
            }
        }

        public int getProcentDecideTaskWithOpen
        {
            get
            {
                try
                {
                    return 100 * TaskWithOpenAnswsExecutions.Where(p => p.Status == StatusTaskExecution.Correct).Count() / TaskWithOpenAnswsExecutions.Count();
                }
                catch
                {
                    return 0;
                }
            }
        }

        public int getProcentChapterDecide
        {
            get
            {
                int first = 0;
                int second = 0;
                try
                {
                    first = 50 * getProcentDecideTaskWithOpen / 100;
                }
                catch { }
                try
                {
                    first = 50 * getMaxProcentTest / 100;
                }
                catch { }
                return first + second;
            }
        }

        public List<TaskExecutionParent> GetTasksExecution()
        {
            var list = new List<TaskExecutionParent>(TaskWithOpenAnswsExecutions.Count() + TaskWithClosedAnswsExecutions.Count());
            foreach(var task in TaskWithOpenAnswsExecutions)
            {
                list.Insert(task.TaskWithOpenAnsw.numericInPack-1, task);
            }
            foreach (var task in TaskWithClosedAnswsExecutions)
            {
                list.Insert(task.TaskWithClosedAnsw.numericInPack - 1, task);
            }
            return list;
        }

    }
}

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
                return getProcentTasks;
            }
        }
        public bool haveFinalTest
        {
            get
            {
                if (TestPack.TestTasks.Count() != 0)
                {
                    return true;
                }
                return false;
            }
        }
        public virtual List<TaskWithOpenAnswsExecution> TaskWithOpenAnswsExecutions { get; set; } = new List<TaskWithOpenAnswsExecution>();

        public virtual List<TaskWithClosedAnswsExecution> TaskWithClosedAnswsExecutions { get; set; } = new List<TaskWithClosedAnswsExecution>();

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

        public int getProcentTasks
        {
            get
            {
                try
                {
                    return 100 *
                        (TaskWithOpenAnswsExecutions.Where(p => p.Status == StatusTaskExecution.Correct).Count() + TaskWithClosedAnswsExecutions.Where(p => p.Status == StatusTaskExecution.Correct).Count())
                        / (TaskWithOpenAnswsExecutions.Count() + TaskWithClosedAnswsExecutions.Count());
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

                    first = 50 * getProcentTasks / 100;
                    if (TestPack.TestTasks.Count() == 0)
                        first *= 2;
                }
                catch { }
                try
                {
                    second = 50 * getMaxProcentTest / 100;
                }
                catch { }

                return first + second;
            }
        }

        public List<TaskExecutionParent> GetTasksExecution()
        {
            var list = new List<TaskExecutionParent>();
            for (int i = 0; i < TaskWithOpenAnswsExecutions.Count() + TaskWithClosedAnswsExecutions.Count(); i++)
                list.Add(null);
            foreach (var task in TaskWithOpenAnswsExecutions)
            {
                list[task.TaskWithOpenAnsw.numericInPack - 1] = task;
                list[task.TaskWithOpenAnsw.numericInPack - 1].isHard = task.TaskWithOpenAnsw.isIncreasedComplexity;
                list[task.TaskWithOpenAnsw.numericInPack - 1].themes = task.TaskWithOpenAnsw.theme;
            }
            foreach (var task in TaskWithClosedAnswsExecutions)
            {
                list[task.TaskWithClosedAnsw.numericInPack - 1] = task;
                list[task.TaskWithClosedAnsw.numericInPack - 1].isHard = task.TaskWithClosedAnsw.isIncreasedComplexity;
                list[task.TaskWithClosedAnsw.numericInPack - 1].themes = task.TaskWithClosedAnsw.theme;
            }
            return list;
        }
    }
}

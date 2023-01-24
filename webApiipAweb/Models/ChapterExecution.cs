using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class ChapterExecution
    {
        [Key]
        public int idChapterExecution { get; set; }

        [ForeignKey("Chapter")]
        public int idChapter { get; set; }

        public virtual Chapter Chapter { get; set; }

        [ForeignKey("SubjectExecution")]
        public int? idSubjectExecution { get; set; }

        public virtual SubjectExecution SubjectExecution { get; set; }

        public virtual List<TestPackExecution> TestPackExecutions { get; set; } = new List<TestPackExecution>();

        public virtual List<SessionChapterExecution> SessionChapterExecutions { get; set; } = new List<SessionChapterExecution>();

        public virtual List<TheorySession> TheorySessions { get; set; } = new List<TheorySession>();

        public int getProcentChapter
        {
            get
            {
                try
                {
                    if (TestPackExecutions.Where(p => p.TestPack.Type == TestPackType.OtherPack).Count() != 0)
                    {
                        return (int)(getProcentOtherTasks + getProcentMainTasks) / 2;
                    }
                    else
                    {
                        return (int)getProcentMainTasks;
                    }
                }
                catch
                {
                    return 0;
                }
            }
        }

        public int getProcentOtherTasks
        {
            get
            {
                try
                {
                    return (int)TestPackExecutions.Where(p => p.TestPack.Type == TestPackType.OtherPack).Average(p => p.getProcentChapterDecide);
                }
                catch
                {
                    return 100;
                }
            }
        }

        public int getProcentMainTasks
        {
            get
            {
                try
                {
                    return (int)TestPackExecutions.Where(p => p.TestPack.Type == TestPackType.MainPack).FirstOrDefault()?.getProcentChapterDecide;
                }
                catch
                {
                    return 0;
                }
            }
        }
    }
}

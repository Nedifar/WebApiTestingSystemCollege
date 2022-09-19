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
        public int getProcentChapter
        {
            get
            {
                try
                {
                    return (int)TestPackExecutions.Average(p => p.getProcentChapterDecide);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public int getProcentChapterTest
        {
            get
            {
                try
                {
                    return (int)TestPackExecutions.Average(p => p.getMaxProcentTest);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public int getProcentChapterTask
        {
            get
            {
                try
                {
                    return (int)TestPackExecutions.Average(p => p.getProcentDecideTaskWithOpen);
                }
                catch
                {
                    return 0;
                }
            }
        }
    }
}

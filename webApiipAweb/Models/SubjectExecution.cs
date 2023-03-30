using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class SubjectExecution
    {
        [Key]
        public string idSubjectExecution { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey("Subject")]
        public int? idSubject { get; set; }

        public virtual Subject Subject { get; set; }

        [ForeignKey("LevelStudingExecution")]
        public string idLevelStudingExecution { get; set; }

        public virtual LevelStudingExecution LevelStudingExecution { get; set; }

        public virtual List<ChapterExecution> ChapterExecutions { get; set; } = new List<ChapterExecution>();

        public string getProgressInProcent
        {
            get
            {
                if (ChapterExecutions.Count != 0)
                    return (int)ChapterExecutions.Average(p => p.getProcentChapter) + "%";
                else
                    return 0 + "%";
            }
        }
    }
}

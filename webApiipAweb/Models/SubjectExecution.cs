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
        public int idSubjectExecution { get; set; }

        [ForeignKey("Subject")]
        public int? idSubject { get; set; }

        public virtual Subject Subject { get; set; }

        [ForeignKey("LevelStudingExecution")]
        public int idLevelStudingExecution { get; set; }

        public virtual LevelStudingExecution LevelStudingExecution { get; set; }

        public virtual List<ChapterExecution> ChapterExecutions { get; set; } = new List<ChapterExecution>();

        public string getProgressInProcent
        {
            get
            {
                return ChapterExecutions.Average(p => p.getProcentChapter) + "%";
            }
        }
    }
}

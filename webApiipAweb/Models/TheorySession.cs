using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webApiipAweb.Models
{
    public class TheorySession
    {
        [Key]
        public int idTheorySession { get; set; }

        public DateTime? beginDate { get; set; }

        public DateTime? endDate { get; set; }

        public bool active { get; set; }

        [ForeignKey("Child")]
        public string idChild { get; set; }

        public virtual Child Child { get; set; }

        [ForeignKey("ChapterExecution")]
        public int idChapterExecution { get; set; }

        public virtual ChapterExecution ChapterExecution { get; set; }
    }
}

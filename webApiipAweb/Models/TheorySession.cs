using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webApiipAweb.Models
{
    public class TheorySession
    {
        [Key]
        public string idTheorySession { get; set; } = Guid.NewGuid().ToString();

        public DateTime? beginDate { get; set; }

        public DateTime? endDate { get; set; }

        public bool active { get; set; }

        [ForeignKey("Child")]
        public string idChild { get; set; }

        public virtual Child Child { get; set; }

        [ForeignKey("ChapterExecution")]
        public string idChapterExecution { get; set; }

        public virtual ChapterExecution ChapterExecution { get; set; }
    }
}

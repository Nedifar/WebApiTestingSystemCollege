using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webApiipAweb.Models
{
    public class SessionChapterExecution
    {
        [Key]
        public string idSessionChapterExecution { get; set; } = Guid.NewGuid().ToString();

        public DateTime? beginDateTime { get; set; }

        public DateTime? endDateTime { get; set; }

        [ForeignKey("Child")]
        public string idChild { get; set; }

        public virtual Child Child { get; set; }

        [ForeignKey("ChapterExecution")]
        public string idChapterExecution { get; set; }

        public virtual ChapterExecution ChapterExecution { get; set; }

        public virtual List<SessionProgress> SessionProgresses { get; set; } = new List<SessionProgress>();

        public bool activeSession { get; set; }
    }
}

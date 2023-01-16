using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class Chapter
    {
        [Key]
        public int idChapter { get; set; }

        public string name { get; set; }

        public string Description { get; set; }

        public bool access { get; set; }

        [ForeignKey("Subject")]
        public int idSubject { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual List<TestPack> TestPacks{ get; set; } = new List<TestPack>();

        public virtual List<ChapterExecution> ChapterExecutions { get; set; } = new List<ChapterExecution>();

        public virtual List<TheoreticalMaterial> TheoreticalMaterials { get; set; } = new List<TheoreticalMaterial>();

        public virtual List<SessionChapterExecution> SessionChapterExecutions { get; set; } = new List<SessionChapterExecution>();
    }
}

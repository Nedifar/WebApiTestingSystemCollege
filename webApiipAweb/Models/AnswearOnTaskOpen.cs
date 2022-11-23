using System.ComponentModel.DataAnnotations;

namespace webApiipAweb.Models
{
    public class AnswearOnTaskOpen
    {
        [Key]
        public int idAnswearOnTaskOpen { get; set; }

        public string answear { get; set; }

        public double mark { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("TaskWithOpenAnsw")]
        public int idTask { get; set; }

        public virtual TaskWithOpenAnsw TaskWithOpenAnsw { get; set; }
    }
}

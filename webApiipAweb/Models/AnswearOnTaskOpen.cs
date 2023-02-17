using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webApiipAweb.Models
{
    public class AnswearOnTaskOpen
    {
        [Key]
        public string idAnswearOnTaskOpen { get; set; } = Guid.NewGuid().ToString();

        public string answear { get; set; }

        public double mark { get; set; }

        [ForeignKey("TaskWithOpenAnsw")]
        public string idTask { get; set; }

        public virtual TaskWithOpenAnsw TaskWithOpenAnsw { get; set; }
    }
}

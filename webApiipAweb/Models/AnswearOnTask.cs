using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class AnswearOnTask
    {
        [Key]
        public string idAnswearOnTask { get; set; } = Guid.NewGuid().ToString();

        public string textAnswear { get; set; }

        public bool accuracy { get; set; }

        [ForeignKey("TestTask")]
        public string idTestTask { get; set; }
        public virtual TestTask TestTask { get; set; }

        [ForeignKey("TaskWithClosedAnsw")]
        public string idTask { get; set; }
        public virtual TaskWithClosedAnsw TaskWithClosedAnsw { get; set; }
    }
}

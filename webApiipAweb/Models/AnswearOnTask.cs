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
        public int idAnswearOnTask { get; set; }
        public string textAnswear { get; set; }
        public bool accuracy { get; set; }
        [ForeignKey("TestTask")]
        public int? idTestTask { get; set; }
        public virtual TestTask TestTask { get; set; }
        [ForeignKey("TaskWithClosedAnsw")]
        public int? idTask { get; set; }
        public virtual TaskWithClosedAnsw TaskWithClosedAnsw { get; set; }
    }
}

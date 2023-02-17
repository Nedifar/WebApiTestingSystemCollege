using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class Solution
    {
        [Key]
        public string idSolution { get; set; } = Guid.NewGuid().ToString();

        public string url { get; set; }

        [ForeignKey("TaskWithOpenAnsw")]
        public string idTaskWithOpenAnsw { get; set; }
        public virtual TaskWithOpenAnsw TaskWithOpenAnsw { get; set; }

        [ForeignKey("TaskWithClosedAnsw")]
        public string idTaskWithClosedAnsw { get; set; }
        public virtual TaskWithClosedAnsw TaskWithClosedAnsw { get; set; }
    }
}

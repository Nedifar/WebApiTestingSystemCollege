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
        public int idSolution { get; set; }

        public string url { get; set; }

        [ForeignKey("TaskWithOpenAnsw")]
        public int idTaskWithOpenAnsw { get; set; }

        [JsonIgnore]
        public virtual TaskWithOpenAnsw TaskWithOpenAnsw { get; set; }
    }
}

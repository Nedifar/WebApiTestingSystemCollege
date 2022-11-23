﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class TestTask
    {
        [Key]
        public int idTestTask { get; set; }
        public string textQuestion { get; set; }
        public virtual List<AnswearOnTask> AnswearOnTasks { get; set; } = new List<AnswearOnTask>();
        [ForeignKey("TestPack")]
        public int? idTestPack { get; set; }
        public virtual TestPack TestPack { get; set; }
    }
}

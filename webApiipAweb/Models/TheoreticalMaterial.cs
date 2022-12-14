using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class TheoreticalMaterial
    {
        [Key]
        public int idTheoreticalMaterial { get; set; }
        public string header { get; set; }
        public string content { get; set; }
        [ForeignKey("Chapter")]
        public int idChapter { get; set; }
        public virtual Chapter Chapter { get; set; }
    }
}

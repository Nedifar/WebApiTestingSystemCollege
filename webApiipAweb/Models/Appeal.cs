using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class Appeal
    {
        [Key]
        public int idAppeal { get; set; }
        [ForeignKey("Child")]
        public string ChildId { get; set; }
        public virtual Child Child { get; set; }
        public DateTime dateAppeal { get; set; }
        [ForeignKey("TypeAppeal")]
        public int idTypeAppeal { get; set; }
        public virtual TypeAppeal TypeAppeal { get; set; }
        public string textAppeal { get; set; }
        public virtual Status status { get; set; }
        public bool inArchive { get; set; }
    }
    public enum Status : byte
    {
        InProcessing = 1,
        Reviewed = 2,
        Received = 3,
        NotReceived = 4
    }
}

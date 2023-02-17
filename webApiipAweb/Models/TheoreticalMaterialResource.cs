using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webApiipAweb.Models
{
    public class TheoreticalMaterialResource
    {
        [Key]
        public string idTheoreticalMaterialResource { get; set; } = Guid.NewGuid().ToString();

        public string url { get; set; }

        public string header { get; set; }

        [ForeignKey("TheoreticalMaterial")]
        public string idTheoreticalMaterial { get; set; }

        public virtual TheoreticalMaterial TheoreticalMaterial { get; set; }
    }
}

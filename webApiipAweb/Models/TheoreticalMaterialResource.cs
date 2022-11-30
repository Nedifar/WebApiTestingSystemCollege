using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webApiipAweb.Models
{
    public class TheoreticalMaterialResource
    {
        [Key]
        public int idTheoreticalMaterialResource { get; set; }

        public string url { get; set; }

        public string header { get; set; }

        [ForeignKey("TheoreticalMaterial")]
        public int idTheoreticalMaterial { get; set; }

        public virtual TheoreticalMaterial TheoreticalMaterial { get; set; }
    }
}

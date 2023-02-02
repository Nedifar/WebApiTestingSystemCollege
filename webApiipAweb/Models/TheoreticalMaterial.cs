using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class TheoreticalMaterial
    {
        [Key]
        public int idTheoreticalMaterial { get; set; }
        public string header { get; set; }
        public string content { get; set; }
        public string additionalMaterial { get; set; }
        [ForeignKey("Chapter")]
        public int idChapter { get; set; }

        [JsonIgnore]
        public virtual Chapter Chapter { get; set; }

        public virtual List<TheoreticalMaterialResource> TheoreticalMaterialResources { get; set; } = new List<TheoreticalMaterialResource>();
    }
}

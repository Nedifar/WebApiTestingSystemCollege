using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace webApiipAweb.Models
{
    public class ParentTask
    {
        [Key]
        public int idTask { get; set; }

        public string textQuestion { get; set; }

        public string theme { get; set; }

        public int numericInPack { get; set; }

        public TypesTask TypesTask { get; set; }

        [ForeignKey("TestPack")]
        public int? idTestPack { get; set; }

        [JsonIgnore]
        public virtual TestPack TestPack { get; set; }

        public bool isIncreasedComplexity { get; set; } = false;

        public virtual List<Solution> Solutions { get; set; } = new List<Solution>();
    }

    public enum TypesTask
    {
        Opened = 1,
        Closed = 2
    }

}

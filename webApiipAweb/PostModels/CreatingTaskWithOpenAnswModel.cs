using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace webApiipAweb.PostModels
{
    public class CreatingTaskWithOpenAnswModel
    {
        public string textQuestion { get; set; }

        public string levelStuding { get; set; }

        public string subjectName { get; set; }

        public string chapter { get; set; }

        public List<AnsModel> answears { get; set; } = new List<AnsModel>();

        public string theme { get; set; }

        public string testPackHeader { get; set; }

        public int? numberInList { get; set; }

        public bool isIncreasedComplexity { get; set; } = false;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ModeCreating mode { get; set; } = ModeCreating.End;

        public virtual List<CreatingSolutionModel> CreatingSolutionModels { get; set; } = new List<CreatingSolutionModel>();
    }

    public enum ModeCreating
    {
        Start,
        End,
        Insert
    }

    public class AnsModel
    {
        public string answear { get; set; }
        public double mark { get; set; }
    }
}

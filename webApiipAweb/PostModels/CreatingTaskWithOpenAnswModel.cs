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
        public string answear { get; set; }
        public string theme { get; set; }
        public string testPackHeader { get; set; }
        public int? numberInList { get; set; }
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
}

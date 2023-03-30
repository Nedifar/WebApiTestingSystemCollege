using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using webApiipAweb.Models;

namespace webApiipAweb.PostModels
{
    public class CreatingTaskWithOpenAnswModel
    {
        public string textQuestion { get; set; }

        public string idTestPack { get; set; }

        public string theme { get; set; }

        public int? numberInList { get; set; }

        public bool isIncreasedComplexity { get; set; } = false;

        public bool orderImportant { get; set; } = true;

        public double fine { get; set; } = 1;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ResultTypes resultType { get; set; }

        public string resultHTML { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ModeCreating mode { get; set; } = ModeCreating.End;

        public string solution { get; set; }

        public AnsModel answear { get; set; }
    }

    public enum ModeCreating
    {
        Start,
        End,
        Insert
    }

    public class AnsModel
    {
        public double mark { get; set; } = 1;

        public string answear { get; set; }
    }
}

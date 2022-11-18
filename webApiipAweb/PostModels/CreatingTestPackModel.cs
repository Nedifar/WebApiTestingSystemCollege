using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace webApiipAweb.PostModels
{
    public class CreatingTestPackModel
    {
        public string header { get; set; }
        public string levelStuding { get; set; }
        public string subjectName { get; set; }
        public string chapterName { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Models.TestPackType Type { get; set; }
    }
}

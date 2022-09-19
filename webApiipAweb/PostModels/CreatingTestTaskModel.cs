using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.PostModels
{
    public class CreatingTestTaskModel
    { 
        public string textQuestion { get; set; }
        public virtual List<CreatingAnswearOnTaskModel> CreatingAnswearOnTaskModels { get; set; } = new List<CreatingAnswearOnTaskModel>();
        public string levelStuding { get; set; }
        public string subjectName { get; set; }
        public string chapterName { get; set; }
        public string testPackHeader { get; set; }
    }
}

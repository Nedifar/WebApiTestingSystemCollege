using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.PostModels
{
    public class CreatingTaskWithOpenAnswModel
    {
        public string textQuestion { get; set; }
        public string levelStuding { get; set; }
        public string subjectName { get; set; }
        public string answear { get; set; }
        public string theme { get; set; }
        public string testPackHeader { get; set; }
        public virtual List<CreatingSolutionModel> CreatingSolutionModels { get; set; } = new List<CreatingSolutionModel>();
    }
}

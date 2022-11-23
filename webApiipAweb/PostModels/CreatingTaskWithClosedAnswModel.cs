using System.Collections.Generic;

namespace webApiipAweb.PostModels
{
    public class CreatingTaskWithClosedAnswModel
    {
        public string textQuestion { get; set; }
        public virtual List<CreatingAnswearOnTaskModel> CreatingAnswearOnTaskModels { get; set; } = new List<CreatingAnswearOnTaskModel>();
        public string levelStuding { get; set; }
        public string subjectName { get; set; }
        public string chapterName { get; set; }
        public string testPackHeader { get; set; }
        public string theme { get; set; }
        public bool isIncreasedComplexity { get; set; } = false;
        public int? numberInList { get; set; }
        public ModeCreating mode { get; set; } = ModeCreating.End;
        public virtual List<CreatingSolutionModel> CreatingSolutionModels { get; set; } = new List<CreatingSolutionModel>();
    }
}

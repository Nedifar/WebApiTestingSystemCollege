using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.PostModels
{
    public class CreatingChapterModel
    {
        public string name { get; set; }
        public string Description { get; set; }
        public string levelStuding { get; set; }
        public bool access { get; set; }
        public string nameSubject { get; set; }
    }

    public class CreatingTheoreticalMaterialModel
    {
        public string chapterName { get; set; }
        public string header { get; set; }
        public string content { get; set; }
        public string levelStuding { get; set; }
        public string nameSubject { get; set; }
    }

    public class DeleteTheoreticalMaterialModel
    {
        public string chapterName { get; set; }
        public string header { get; set; }
        public string levelStuding { get; set; }
        public string nameSubject { get; set; }
    }
}

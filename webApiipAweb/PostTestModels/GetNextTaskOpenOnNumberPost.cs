using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.PostTestModels
{
    public class GetNextTaskOpenOnNumberPost
    {
        public int idExecChapter { get; set; }
        public int currentSerialNumber { get; set; }
        public string theme { get; set; }
        public string testPackHeader { get; set; }
    }
}

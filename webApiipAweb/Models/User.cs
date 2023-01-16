using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webApiipAweb.Models
{
    public class Child : IdentityUser
    {
        public string lastName { get; set; }

        public string firstName { get; set; }

        public int levelStuding { get; set; }

        public double point { get; set; }   

        public double spendPoint { get; set; }

        public string passRecoveryCode { get; set; }

        public string imagePath { get; set; }

        public string getRelevantPathImage
        {
            get
            {
                if(imagePath != null)
                return Convert.ToBase64String(System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + imagePath));
                else
                    return null;
            }
        }

        public virtual List<ThingPackExecution> ThingPackExecutions { get; set; } = new List<ThingPackExecution>();

        public virtual List<AchivmentExecution> AchivmentExecutions { get; set; } = new List<AchivmentExecution>();

        public virtual List<LevelStudingExecution> LevelStudingExecutions { get; set; } = new List<LevelStudingExecution>();

        public virtual List<Appeal> Appeals { get; set; } = new List<Appeal>();

        public virtual List<SessionChapterExecution> SessionChapterExecutions { get; set; } = new List<SessionChapterExecution>();
    }
}

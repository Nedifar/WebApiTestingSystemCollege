﻿using Microsoft.AspNetCore.Identity;
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

        public string levelWord { get; set; }

        public double point { get; set; }   

        public double spendPoint { get; set; }

        public string passRecoveryCode { get; set; }

        public string imagePath { get; set; }

        public string refreshToken { get; set; }

        public DateTime? refreshTokenExpiryTime { get; set; }

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

        public virtual List<TheorySession> TheorySessions { get; set; } = new List<TheorySession>();

        [ForeignKey("School")]
        public string idSchool { get; set; }

        public virtual School School { get; set; }

        [ForeignKey("Municipality")]
        public int? idMunicipality { get; set; }

        public virtual Municipality Municipality { get; set; }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }

    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual Child User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}

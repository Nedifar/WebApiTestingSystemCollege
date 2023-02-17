using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webApiipAweb.Models
{
    public class School
    {
        [Key]
        public string idSchool { get; set; } = Guid.NewGuid().ToString();

        public string nameSchool { get; set; }

        [ForeignKey("Municipality")]
        public int idMunicipality { get; set; }

        public virtual Municipality Municipality { get; set; }

        public virtual List<Child> Users { get; set; } = new List<Child>();

        //public virtual List<Child> Teachers { get; set; } = new List<Child>();

        //public virtual List<Child> SchoolAdmins { get; set; } = new List<Child>();

    }
}

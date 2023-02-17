using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webApiipAweb.Models
{
    public class Municipality
    {
        [Key]
        public int idMunicipality { get; set; }

        public string name { get; set; }

        [ForeignKey("Area")]
        public string idArea { get; set; }

        public virtual Area Area { get; set; }

        public virtual List<School> Schools { get; set; } = new List<School>();

        public virtual List<Child> MunicipalityAdmins { get; set; } = new List<Child>();
    }
}

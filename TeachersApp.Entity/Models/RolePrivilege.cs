using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.Enum;

namespace TeachersApp.Entity.Models
{
    public class RolePrivilege
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RolePrivilegeID { get; set; }

        [ForeignKey("Role")]
        public int RoleID { get; set; }

        [ForeignKey("Privilege")]
        public int PrivilegeID { get; set; }



        public Role Role { get; set; } = null!;
        public Privilege Privilege { get; set; } = null!;
    }
}

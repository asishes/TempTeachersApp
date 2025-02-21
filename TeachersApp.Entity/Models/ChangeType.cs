using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class ChangeType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChangeTypeID { get; set; }

        [Required, MaxLength(50)]
        public string ChangeText { get; set; } = null!;




        public ICollection<TeacherHistory> TeacherHistories { get; set; } = new List<TeacherHistory>();

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class EmployeeCasteCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CasteCategoryID {  get; set; }

        [Required, MaxLength(50)]
        public string CasteCategoryName { get; set; } = null!;


        public ICollection<EmployeePersonalDetails> PersonalDetails { get; set; } = new List<EmployeePersonalDetails>();

    }
}

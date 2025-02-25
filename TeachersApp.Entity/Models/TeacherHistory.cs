using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class TeacherHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HistoryID { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeID { get; set; }

        [Required] 
        public DateTime? ChangeDate { get; set; }

        [ForeignKey("User")]
        public int? ChangedByID { get; set; }

        [Required]
        public string? ChangeDescription { get; set; } = null!;

        [ForeignKey("ChangeType")]
        public int? ChangeTypeID { get; set; }

        [ForeignKey("School")]
        public int? ChangeFromSchoolID { get; set; }

        [ForeignKey("School")]
        public int? ChangeToSchoolID { get; set; }

        [ForeignKey("Designation")]
        public int? PromotedFromPositionID { get; set; }

        [ForeignKey("Designation")]
        public int? PromotedToPositionID { get; set; }




        public Employee Employee { get; set; } = null!;
        public User ChangedByUser { get; set; } = null!;
        public ChangeType ChangeType { get; set; } = null!;
        public School SchoolFrom { get; set; } = null!;
        public School SchoolTo { get; set; } = null!;
        public Designation PromotedFromDesignation { get; set; } = null!;
        public Designation PromotedToDesignation { get; set; } = null!;



    }
}

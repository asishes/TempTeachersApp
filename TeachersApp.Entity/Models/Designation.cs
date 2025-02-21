    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace TeachersApp.Entity.Models
    {
        public class Designation
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int DesignationID { get; set; }

            [Required, MaxLength(100)]
            public string DesignationText { get; set; } = null!;





            public ICollection<Employee> Employees { get; set; } = new List<Employee>();
            public ICollection<Promotion> PromotionsFrom { get; set; } = new List<Promotion>();
            public ICollection<Promotion> PromotionsTo { get; set; } = new List<Promotion>();
            public ICollection<SchoolPosition> SchoolPositions { get; set; } = new List<SchoolPosition>();
            public ICollection<TeacherHistory> TeacherHistoryPromotionsFrom { get; set; } = new List<TeacherHistory>();
            public ICollection<TeacherHistory> TeacherHistoryPromotionsTo { get; set; } = new List<TeacherHistory>();
            public ICollection<DesignationQualification> DesignationQualifications { get; set; } = new List<DesignationQualification>();
            public ICollection<SchoolTypeDesignation> SchoolTypeDesignations { get; set; } = new List<SchoolTypeDesignation>();


    }
    }

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeID { get; set; }

        [MaxLength(50)]
        public string UniqueID { get; set; } = null!;

        [ForeignKey("EmployeeType")]
        public int? EmployeeTypeID { get; set; }

        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(15)]
        public string? Phone { get; set; }

        [MaxLength(255)]
        public string? PresentAddress { get; set; }

        [MaxLength(255)]
        public string? PermanentAddress { get; set; }

        [ForeignKey("Photo")]
        public int? PhotoID { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public DateTime? WorkStartDate { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? RetirementDate { get; set; }

        [ForeignKey("Designation")]
        public int? DesignationID { get; set; }

        [ForeignKey("Category")]
        public int? CategoryID { get; set; }

        [ForeignKey("School")]
        public int? SchoolID { get; set; }

        [ForeignKey("Subject")]
        public int? SubjectID { get; set; }

        [ForeignKey("SchoolPosition")]
        public int? SchoolPositionID { get; set; }

        [ForeignKey("Status")]
        public int? StatusID { get; set; }

        [ForeignKey("ApprovalType")]
        public int? ApprovalTypeID { get; set; }
        public string? ApprovalTypeReason { get; set; } 

        [ForeignKey("Supervisor")]
        public int? SupervisorID { get; set; }

        public bool PromotionEligible { get; set; }

        public int? PromotionSeniorityNumber { get; set; } // This is the promotion seniority


        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


        public EmployeeCategory EmployeeCategory { get; set; } = null!;
        public EmployeeType EmployeeType { get; set; } = null!;
        public Photo Photo { get; set; } = null!;
        public Designation Designation { get; set; } = null!;
        public School School { get; set; } = null!;
        public Status Status { get; set; } = null!;
        public Subject Subject { get; set; } = null!;
        public ApprovalType ApprovalType { get; set; } = null!;
        public Employee? Supervisor { get; set; } = null!;
        public SchoolPosition SchoolsPosition { get; set; } = null!;
        public EmployeePersonalDetails PersonalDetails { get; set; } = null!;
        public User User { get; set; } = null!;

        public ICollection<School> Schools { get; set; } = new List<School>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<TransferRequest> TransferRequests { get; set; } = new List<TransferRequest>();
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
        public ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
        public ICollection<TeacherHistory> TeacherHistories { get; set; } = new List<TeacherHistory>();
        public ICollection<EmployeeEducation> EmployeeEducations { get; set; } = new List<EmployeeEducation>();
        public ICollection<EmployeeDocument> EmployeeDocuments { get; set; } = new List<EmployeeDocument>();
        public ICollection<PromotionRelinquishment> PromotionRelinquishments { get; set; } = new List<PromotionRelinquishment>();
    }
}

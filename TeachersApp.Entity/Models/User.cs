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
    public class User
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        [Required, MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        [ForeignKey("Employee")]
        public int? EmployeeID { get; set; }

        [ForeignKey("Role")]
        public int RoleID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }



        public Role Role { get; set; } = null!;
        public Employee Employee { get; set; } = null!;


        public ICollection<TransferRequest> TransferRequestedByUser { get; set; } = new List<TransferRequest>();
        public ICollection<TransferRequest> TransferApprovedByUser { get; set; } = new List<TransferRequest>();
        public ICollection<LeaveRequest> LeaveRequestedByUser { get; set; } = new List<LeaveRequest>();
        public ICollection<LeaveRequest> LeaveApprovedByUser { get; set; } = new List<LeaveRequest>();
        public ICollection<Promotion> PromotionRequestedByUser { get; set; } = new List<Promotion>();
        public ICollection<Promotion> PromotionApprovedByUser { get; set; } = new List<Promotion>();
        public ICollection<TeacherHistory> TeacherHistoryChangedByUser { get; set; } = new List<TeacherHistory>();



    }
}


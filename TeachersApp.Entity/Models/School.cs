using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class School
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SchoolID { get; set; }

        [Required, MaxLength(100)]
        public string SchoolName { get; set; } = null!;

        [MaxLength(255)]
        public string Address { get; set; } = null!;

        [ForeignKey("City")]
        public int CityID { get; set; }

        [Required, MaxLength(100)]
        public string State { get; set; } = null!;

        [Required]
        [Range(600000, 699999)]
        public string Pincode { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(15)]
        public string Phone { get; set; } = null!;

        [ForeignKey("Photo")]
        public int? PhotoID { get; set; }

        [ForeignKey("Status")]
        public int StatusID { get; set; }

        [ForeignKey("Employee")]
        public int? PrincipalID { get; set; }

        [ForeignKey("Employee")]
        public int? VicePrincipalID { get; set; }



        public City City { get; set; } = null!;
        public Photo Photo { get; set; } = null!;
        public Status Status { get; set; } = null!;
        public Employee Principal { get; set; } = null!;
        public Employee VicePrincipal { get; set; } = null!;

        public ICollection<SchoolClass> SchoolClasses { get; set; } = new List<SchoolClass>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<TransferRequest> TransferRequestsFrom { get; set; } = new List<TransferRequest>();
        public ICollection<TransferRequest> TransferRequestsToOne { get; set; } = new List<TransferRequest>();
        public ICollection<TransferRequest> TransferRequestsToTwo { get; set; } = new List<TransferRequest>();
        public ICollection<TransferRequest> TransferRequestsToThree { get; set; } = new List<TransferRequest>();
        public ICollection<TransferRequest> TransferRequestsToApproved { get; set; } = new List<TransferRequest>();
        public ICollection<TeacherHistory> TeacherHistoriesFrom { get; set; } = new List<TeacherHistory>();
        public ICollection<TeacherHistory> TeacherHistoriesTo { get; set; } = new List<TeacherHistory>();
        public ICollection<SchoolPosition> SchoolPositions { get; set; } = new List<SchoolPosition>();
        public ICollection<Promotion> PromotionRequestsFromSchool { get; set; } = new List<Promotion>();
        public ICollection<Promotion> PromotionRequestsToApprovedSchool { get; set; } = new List<Promotion>();
        public ICollection<SchoolStandardType> SchoolStandardTypes { get; set; } = new List<SchoolStandardType>();
    }
}

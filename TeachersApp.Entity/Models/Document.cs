using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TeachersApp.Entity.Models
{
    public class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentID { get; set; }

        [Required, MaxLength(255)]
        public string DocumentText { get; set; } = null!;

        [NotMapped]
        public IFormFile? DocumentFile { get; set; }

        public string DocumentFileName { get; set; } = null!;

        [ForeignKey("Status")]
        public int StatusID { get; set; }




        public Status Status { get; set; } = null!;

        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
        public ICollection<EmployeeEducation> EmployeeEducations { get; set; } = new List<EmployeeEducation>();
        public ICollection<EmployeeDocument> EmployeeDocuments { get; set; } = new List<EmployeeDocument>();
        public ICollection<PromotionRelinquishment> PromotionRelinquishments { get; set; } = new List<PromotionRelinquishment>();
    }
}

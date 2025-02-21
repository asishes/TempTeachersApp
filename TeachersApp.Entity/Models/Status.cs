using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class Status
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusID { get; set; }

        [Required, MaxLength(50)]
        public string StatusText { get; set; } = null!;

        [Required, MaxLength(50)]
        public string StatusType { get; set; } = null!;



        public ICollection<School> Schools { get; set; } = new List<School>();
        public ICollection<TransferRequest> TransferRequests { get; set; } = new List<TransferRequest>();
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
        public ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<SchoolPosition> SchoolPositions { get; set; } = new List<SchoolPosition>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}

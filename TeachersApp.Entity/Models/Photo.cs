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
    public class Photo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PhotoID { get; set; }

        [NotMapped]
        public IFormFile? PhotoFile { get; set; }

        public string? PhotoImageName { get; set; }



        public ICollection<School> Schools { get; set; } = new List<School>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}

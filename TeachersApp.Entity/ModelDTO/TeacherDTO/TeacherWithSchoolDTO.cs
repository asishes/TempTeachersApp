using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.Models;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class TeacherWithSchoolDTO
    {

        public int TeacherID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SubjectID { get; set; }
        public string Subject { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? DateOfRetiring { get; set; }
        public string Address { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string ReportsTo { get; set; } = string.Empty;
        public DateTime DateOfJoin { get; set; }
        public List<TransferHistoryDTO> TransferHistory { get; set; } = new List<TransferHistoryDTO>();
        public SchoolDTO School { get; set; } = new SchoolDTO();
        public string? Error { get; set; }
    }

    public class TransferHistoryDTO
    {
        public string FromSchool { get; set; } = string.Empty;
        public string ToSchool { get; set; } = string.Empty;
        public DateTime TransferDate { get; set; }
    }

    public class SchoolDTO
    {
        public int SchoolID { get; set; }
        public string SchoolName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public PrincipalDTO Principal { get; set; } = new PrincipalDTO();
        public VicePrincipalDTO VicePrincipal { get; set; } = new VicePrincipalDTO();
    }

    public class PrincipalDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
    }

    public class VicePrincipalDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
    }
}


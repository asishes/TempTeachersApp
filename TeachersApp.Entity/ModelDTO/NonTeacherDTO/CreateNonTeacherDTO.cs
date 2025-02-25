using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.TeacherDTO;

namespace TeachersApp.Entity.ModelDTO.NonTeacherDTO
{
    public class CreateNonTeacherDTO 
    {
        public string? PEN { get; set; }
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
        public int? SexID { get; set; }
        public DateTime? DateofBirth { get; set; }
        public DateTime? RetirementDate { get; set; }
        public string? Phone { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public int? ReligionID { get; set; }
        public int? CasteID { get; set; }
        public string? Caste { get; set; } = string.Empty;
        public int? BloodGroupID { get; set; }
        public bool? DifferentlyAbled { get; set; }
        public bool? ExServiceMen { get; set; }
        public string? IdentificationMark1 { get; set; }
        public string? IdentificationMark2 { get; set; }
        public double? Height { get; set; }
        public string? AadhaarID { get; set; } = string.Empty;
        public string? PanID { get; set; } = null!;
        public string? VoterID { get; set; } = null!;
        public string? RationID { get; set; } = null!;
        public string? PresentAddress { get; set; } = string.Empty!;
        public string? PermanentAddress { get; set; } = string.Empty;
        public string? FatherName { get; set; } = string.Empty!;
        public string? MotherName { get; set; } = string.Empty!;
        public bool? InterReligion { get; set; }
        public int? MaritalStatusID { get; set; }
        public string? SpouseName { get; set; }
        public int? SpouseReligionID { get; set; }
        public string? SpouseCaste { get; set; }
        public int? PhotoID { get; set; }
        public int? SchoolPositionID { get; set; }
        public int? ApprovalTypeID { get; set; }
        public string? ApprovalTypeReason { get; set; }
        public bool? PromotionEligible { get; set; }
        public int? DepartmentID { get; set; }
        public int? DistrictID { get; set; }
        public string? PFNummber { get; set; }
        public string? PRAN { get; set; }
        public DateTime? DateofJoin { get; set; }
        public DateTime? DateofJoinDepartment { get; set; }
        public int? SchoolID { get; set; }
        public int? CategoryID { get; set; }
        public bool? ProtectedTeacher { get; set; }
        public int? DesignationID { get; set; }

        public string? PasswordHash { get; set; } = string.Empty;

        public List<EmployeeEducationDTO?> Educations { get; set; } = new List<EmployeeEducationDTO?>();

        public List<EmployeeDocumentDTO?> EmployeeDocuments { get; set; } = new List<EmployeeDocumentDTO?>();
    }
}

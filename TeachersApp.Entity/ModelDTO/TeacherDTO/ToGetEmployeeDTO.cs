using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.Models;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class ToGetEmployeeDTO
    {
        public int EmployeeID { get; set; }
        public string UniqueID { get; set; } = string.Empty;
        public int? DepartmentID{ get; set; }
        public string? DepartmentName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; } 
        public string? PresentAddress { get; set; } 
        public string? PermanentAddress { get; set; } 
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateofDepartmentJoin { get; set; }
        public DateTime? DateofJoin { get; set; }
        public DateTime? RetirementDate { get; set; }
        public int? DesignationID { get; set; }

        public string? DesignationName { get; set; }
        public int? CategoryID { get; set; }

        public string? CategoryName { get; set; }   
        public int? SchoolID { get; set; }
        public int? PhotoID { get; set; }
        public string? Photopath { get; set; }  
        public string? SchoolName { get; set; }
        public int? SubjectID { get; set; }

        public string? SubjectName { get; set; }
        public int? SchoolPositionID { get; set; }

        public int? StatusID { get; set; }

        public string? StatusName { get; set; }
        public int? ApprovalTypeID { get; set; }

        public string? ApprovalTypeName { get; set; }
        public int? SupervisorID { get; set; }

        public string? SuperVisorName { get; set; }  
        public bool PromotionEligible { get; set; }
        public int? SexID { get; set; }

        public string? Gender {  get; set; }
        public int? ReligionID { get; set; }

        public string? ReligionName { get; set; }
        public string? Caste { get; set; }
        public int? CasteID { get; set; }

        public string? CasteName { get; set; }
        public int? BloodGroupID { get; set; }

        public string? BloodName { get; set; }
        public int? DistrictID { get; set; }

        public string? District {  get; set; }
        public bool? DifferentlyAbled { get; set; } 
        public bool? ExServiceMen { get; set; }
        public string? IdentificationMark1 { get; set; }
        public string? IdentificationMark2 { get; set; }
        public double? Height { get; set; }
        public string? FatherName { get; set; } 
        public string? MotherName { get; set; } 
        public bool? InterReligion { get; set; }
        public int? MaritalStatusID { get; set; }

        public string? MaritalStatusName { get; set; }
        public string? SpouseName { get; set; }
        public int? SpouseReligionID { get; set; }
        public string? SpouseReligionName { get; set; }
        public string? SpouseCaste { get; set; } 
        public string? PanID { get; set; } 
        public string? VoterID { get; set; } 
        public string? AadhaarID { get; set; } 
        public string? PFNumber { get; set; }
        public string? PRAN { get; set; }
        public string? PEN { get; set; }
        public bool? EligibilityTestQualified { get; set; }
        public bool? ProtectedTeacher { get; set; }

   
        // List of EmployeeEducations
        public List<GetEmployeeEducationDTO> GetEducations { get; set; } = new List<GetEmployeeEducationDTO>();
        
        public List<GetEmployeeDocumentDTO> GetEmployeeDocuments { get; set; } = new List<GetEmployeeDocumentDTO>();
    }
}


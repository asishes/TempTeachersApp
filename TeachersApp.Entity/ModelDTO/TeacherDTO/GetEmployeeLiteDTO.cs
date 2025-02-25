using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.GetPersonalDetailsDTO;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class GetEmployeeLiteDTO
    {
        public int EmployeeID { get; set; }
        public string UniqueID { get; set; } = string.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;      
        public string? PresentAddress { get; set; } = string.Empty;
        public string? PermanentAddress { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateofDepartmentJoin { get; set; }
        public DateTime? DateofJoin { get; set; }
        public DateTime? RetirementDate { get; set; }
        public int? SchoolPositionID { get; set; }
        public bool? PromotionEligible { get; set; }
        public string? ApprovalTypeReason { get; set; } = string.Empty;
        public string? CasteName { get; set; } = string.Empty;
        public bool? DifferentlyAbled { get; set; }
        public bool? ExServiceMen { get; set; }
        public string? IdentificationMark1 { get; set; } = string.Empty;
        public string? IdentificationMark2 { get; set; } = string.Empty;
        public double? Height { get; set; }
        public string? FatherName { get; set; } = string.Empty;
        public string? MotherName { get; set; } = string.Empty;
        public bool? InterReligion { get; set; }
        public string? SpouseName { get; set; } = string.Empty;
        public string? SpouseCaste { get; set; } = string.Empty;
        public string? PanID { get; set; } = string.Empty;
        public string? VoterID { get; set; } = string.Empty;
        public string? AadhaarID { get; set; } = string.Empty;
        public string? PFNumber { get; set; } = string.Empty;
        public string? PRAN { get; set; } = string.Empty;
        public string? PEN { get; set; } = string.Empty;
        public bool? EligibilityTestQualified { get; set; }
        public bool? ProtectedTeacher { get; set; }

        public DateTime CreatedDate { get; set; }
        public GetEmployeeTypeDTO? Department { get; set; }
        public GetDesignationDTO? designationDTO { get; set; }
        public GetEmployeeCategoryDTO? GetEmployeeCategoryDTO { get; set; }
        public GetSchoolDTO? schoolDTO { get; set; }
        public GetPhotoDTO? photoDTO { get; set; }
        public GetSubjectDTO? getSubjectDTO { get; set; }
        public GetStatusDTO? statusDTO { get; set; }
        public GetApprovalTypeDTO? GetApprovalTypeDTO { get; set; }
        public GetEmployeeDTO? Supervisor { get; set; }
        public GetGenderDTO? genderDTO { get; set; }
        public GetReligionDTO? religionDTO { get; set; }
        public GetCasteCategoryDTO? casteCategoryDTO { get; set; }
        public GetBloodGroupDTO? bloodGroupDTO { get; set; }
        public GetDistrictDTO? districtDTO { get; set; }
        public GetMaritalStatusDTO? MaritalStatusDTO { get; set; }
        public GetReligionDTO? SpouseReligionDTO { get; set; }

        // List of EmployeeEducations
        public List<GetEmployeeEducationDTO> GetEducations { get; set; } = new List<GetEmployeeEducationDTO>();

        public List<GetEmployeeDocumentDTO> GetEmployeeDocuments{ get; set; } = new List<GetEmployeeDocumentDTO>();

    }
}

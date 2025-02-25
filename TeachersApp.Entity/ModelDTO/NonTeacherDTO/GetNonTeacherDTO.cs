using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.GetPersonalDetailsDTO;
using TeachersApp.Entity.ModelDTO.TeacherDTO;

namespace TeachersApp.Entity.ModelDTO.NonTeacherDTO
{
    public class GetNonTeacherDTO
    {
        public int EmployeeID { get; set; }
        public string UniqueID { get; set; } = string.Empty;
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
        public int? SchoolPositionID { get; set; }
        public string? ApprovalTypeReason { get; set; }
        public bool? PromotionEligible { get; set; }
        public string? CasteName { get; set; }
        public bool? DifferentlyAbled { get; set; }
        public bool? ExServiceMen { get; set; }
        public string? IdentificationMark1 { get; set; }
        public string? IdentificationMark2 { get; set; }
        public double? Height { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public bool? InterReligion { get; set; }
        public string? SpouseName { get; set; }
        public string? SpouseCaste { get; set; }
        public string? PanID { get; set; }
        public string? VoterID { get; set; }
        public string? AadhaarID { get; set; }
        public string? PFNumber { get; set; }
        public string? PRAN { get; set; }
        public string? PEN { get; set; }
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

        public List<GetEmployeeDocumentDTO> GetEmployeeDocuments { get; set; } = new List<GetEmployeeDocumentDTO>();
    }
}

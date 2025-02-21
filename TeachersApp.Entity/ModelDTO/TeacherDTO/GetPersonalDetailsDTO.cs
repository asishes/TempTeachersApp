using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.ModelDTO.GetPersonalDetailsDTO;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class GetPersonalDetailsDTO
    { 
            public int EmployeeDetailID { get; set; }
            public int EmployeeID { get; set; }
            public int SexID { get; set; }
            public int ReligionID { get; set; }
            public string Caste { get; set; } = string.Empty;
            public int CasteID { get; set; }
            public int BloodGroupID { get; set; }
            public int ServiceCategoryID { get; set; }
            public int DistrictID { get; set; }
            public bool DifferentlyAbled { get; set; } = false;
            public bool ExServiceMen { get; set; }
            public string IdentificationMark1 { get; set; } = string.Empty;
            public string IdentificationMark2 { get; set; } = string.Empty;
            public double Height { get; set; }
            public string FatherName { get; set; } = string.Empty;
            public string MotherName { get; set; } = string.Empty;
            public bool InterReligion { get; set; }
            public int MaritalStatusID { get; set; }
            public string? SpouseName { get; set; }
            public int? SpouseReligionID { get; set; }
            public string? SpouseCaste { get; set; } = string.Empty;
            public string PanID { get; set; } = string.Empty;
            public string VoterID { get; set; } = string.Empty;
            public string AadhaarID { get; set; } = string.Empty;
            public string? PFNumber { get; set; }
            public string? PRAN { get; set; }
            public string? PEN { get; set; }
            public bool EligibilityTestQualified { get; set; }
            public bool ProtectedTeacher { get; set; }


            // Navigation properties for DTO
            public GetEmployeeDTO Employee { get; set; } = null!;
            public GetGenderDTO Sex { get; set; } = null!;
            public GetReligionDTO EmployeeReligion { get; set; } = null!;
            public GetCasteCategoryDTO CasteCategory { get; set; } = null!;
            public GetBloodGroupDTO BloodGroup { get; set; } = null!;
            public GetMaritalStatusDTO MaritalStatus { get; set; } = null!;
            public GetReligionDTO EmployeeSpouseReligion { get; set; } = null!;
            public GetDistrictDTO District { get; set; } = null!;
        }
    }



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.TeacherDTO
{
    public class TeacherUpdateDTO
    {
        public string? PEN { get; set; } // Employee identifier, if needed for update
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? SexID { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int? ReligionID { get; set; }
        public int? CasteID { get; set; }
        public string? Caste { get; set; }
        public int? BloodGroupID { get; set; }
        public bool? DifferentlyAbled { get; set; }
        public bool? ExServiceMen { get; set; }
        public string? IdentificationMark1 { get; set; }
        public string? IdentificationMark2 { get; set; }
        public double? Height { get; set; }
        public string? AadhaarID { get; set; }
        public string? PanID { get; set; }
        public string? VoterID { get; set; }
        public string? PresentAddress { get; set; }
        public string? PermanentAddress { get; set; }
        public string? FatherName { get; set; }
        public string? MotherName { get; set; }
        public bool? InterReligion { get; set; }
        public int? MaritalStatusID { get; set; }
        public string? SpouseName { get; set; }
        public int? SpouseReligionID { get; set; }
        public string? SpouseCaste { get; set; }
        public int? SchoolPositionID { get; set; }
        public bool PromotionEligible { get; set; }
        public int? DepartmentID { get; set; }
        public int? DistrictID { get; set; }
        public string? PFNummber { get; set; }
        public string? PRAN { get; set; }
        public DateTime? DateofJoin { get; set; }
        public DateTime? DateofJoinDepartment { get; set; }
        public DateTime? DateofRetirement { get; set; }
        public int? SchoolID { get; set; }
        public int? PhotoID { get; set; }
        public int? CategoryID { get; set; }
        public int? ApprovalTypeID { get; set; }
        public string? ApprovalTypeReason { get; set; }
        public bool? ProtectedTeacher { get; set; }
        public int? DesignationID { get; set; }
        public int? SubjectID { get; set; }
        public bool? EligibilityTestQualified { get; set; }





        // Nested list for education records
        public List<EmployeeEducationDTO?> Educations { get; set; } = new List<EmployeeEducationDTO?>();

        public List<EmployeeDocumentDTO?> EmployeeDocuments { get; set; } = new List<EmployeeDocumentDTO?>();
    }
}

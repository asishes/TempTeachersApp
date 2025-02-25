using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.Models
{
    public class EmployeePersonalDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeDetailID { get; set; }

        [ForeignKey("Employee")]
        public int? EmployeeID { get; set; }

        [ForeignKey("EmployeeSex")]
        public int? SexID { get; set; }

        [ForeignKey("EmployeeReligion")]
        public int? ReligionID { get; set; }

        [MaxLength(50)]
        public string? Caste { get; set; }

        [ForeignKey("EmployeeCasteCategory")]
        public int? CasteID { get; set; }

        [ForeignKey("EmployeeBloodGroup")]
        public int? BloodGroupID { get; set; }


        [ForeignKey("District")]
        public int? DistrictID { get; set; }

        public bool? DifferentlyAbled { get; set; } = false;

        public bool? ExServiceMen { get; set; }

        [MaxLength(100)]
        public string? IdentificationMark1 { get; set; }

        [MaxLength(100)]
        public string? IdentificationMark2 { get; set; }

        public double? Height { get; set; }

        [MaxLength(50)]
        public string? FatherName { get; set; }

        [MaxLength(50)]
        public string? MotherName { get; set; } 

        public bool? InterReligion { get; set; } 

        [ForeignKey("EmployeeMaritalStatus")]
        public int? MaritalStatusID { get; set; }

        [MaxLength(50)]
        public string? SpouseName { get; set; } // Consider making this non-nullable if required

        [ForeignKey("EmployeeSpouseReligion")]
        public int? SpouseReligionID { get; set; }

        [MaxLength(50)]
        public string? SpouseCaste { get; set; }

        [MaxLength(50)]
        public string? PanID { get; set; }

        [MaxLength(50)]
        public string? VoterID { get; set; } 

        [MaxLength(50)]
        public string? AadhaarID { get; set; } 

        public string? PFNummber { get; set; }

        public string? PRAN { get; set; }

        public string? PEN { get; set; }

        public bool? EligibilityTestQualified { get; set; }

        public bool? ProtectedTeacher { get; set; }



        // Navigation properties
        public Employee Employee { get; set; } = null!;
        public EmployeeSex Sex { get; set; } = null!;
        public EmployeeReligion EmployeeReligion { get; set; } = null!;
        public EmployeeCasteCategory CasteCategory { get; set; } = null!;
        public EmployeeBloodGroup BloodGroup { get; set; } = null!;
        public EmployeeMaritalStatus MaritalStatus { get; set; } = null!;
        public EmployeeReligion EmployeeSpouseReligion { get; set; } = null!;
        public District District { get; set; } = null!;


    }
}

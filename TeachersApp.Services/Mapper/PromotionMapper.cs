using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.Migrations;
using TeachersApp.Entity.ModelDTO.GetPersonalDetailsDTO;
using TeachersApp.Entity.ModelDTO.LeaveRequestDTO;
using TeachersApp.Entity.ModelDTO.PromotionDTO;
using TeachersApp.Entity.ModelDTO.PromotionDTO.PromotionRelinquishment;
using TeachersApp.Entity.ModelDTO.SchoolDTO;
using TeachersApp.Entity.ModelDTO.TeacherDTO;
using TeachersApp.Entity.ModelDTO.TransferRequestDTO;
using TeachersApp.Entity.Models;

namespace TeachersApp.Services.Mapper
{
    public static class PromotionMapper
    {
        private static int CalculateExperience(DateTime? hireDate, DateTime currentDate)
        {
            if (hireDate == null)
                return 0;

            var startDate = hireDate.Value;
            int experience = currentDate.Year - startDate.Year;

            // If the current date is before the anniversary of the hire date in the current year, subtract 1
            if (currentDate < startDate.AddYears(experience))
                experience--;

            return experience;
        }

        private static int CalculateExactAge(DateTime? dateOfBirth, DateTime currentDate)
        {
            if (dateOfBirth == null)
            {
                // Return 0 or throw an exception if age cannot be calculated without a date of birth
                return 0;
            }

            DateTime dob = dateOfBirth.Value;
            int age = currentDate.Year - dob.Year;
            int monthDifference = currentDate.Month - dob.Month;
            int dayDifference = currentDate.Day - dob.Day;

            // Adjust for month and day
            if (monthDifference < 0 || (monthDifference == 0 && dayDifference < 0))
            {
                age--;
            }

            return age;
        }
        public static GetPromotionListDTO ToGetPromotionEmployeeDTO(this Employee employee)
        {
            return new GetPromotionListDTO
            {
                ID=employee.EmployeeID,
                Name= $"{employee.FirstName} {employee.LastName}",
                SchoolID =employee.SchoolID ?? null,
                School = employee.School?.SchoolName ?? "Unknown School",  // Handle null School
                SubjectID = employee.SubjectID,
                Subject = employee.Subject?.SubjectText ?? "Unknown Subject",  // Handle null Subject
                DesignationID = employee.DesignationID,
                Designation = employee.Designation?.DesignationText ?? "Unknown Designation",  // Handle null Designation
                SeniorityNumber =employee.PromotionSeniorityNumber,
            };
        }

        public static Promotion ToCreatePromotionRequest(this CreatePromotionRequestDTO createPromotionRequest)
        {
            var transferRequest = new Promotion
            {
                EmployeeID = createPromotionRequest.EmployeeID,
                RequestorComment = createPromotionRequest.RequestorComment,
                FilePath = createPromotionRequest.FilePath,
            };
            return transferRequest;
        }

        public static GetPromotionRequestDTO ToGetPromotionRequestDTO(this Promotion promotionRequest)
        {
            return new GetPromotionRequestDTO
            {
                PromotionID = promotionRequest.PromotionID,
                EmployeeID = promotionRequest.EmployeeID,
                EmployeeName = $"{promotionRequest.Employee.FirstName ?? "Unknown"} {promotionRequest.Employee.LastName ?? "Unknown"}",
                PromotionDate = promotionRequest.PromotionDate,
                PromotedFromID = promotionRequest.PromotedFromDesignationID,
                PromotedFromDesignation = promotionRequest.PromotedFromDesignation?.DesignationText,
                PromotedToID = promotionRequest.PromotedToDesignationID,
                PromotedToDesignation = promotionRequest.PromotedToDesignation?.DesignationText,
                FromSchoolID = promotionRequest.FromSchoolID,
                PromotedFromSchool = promotionRequest.FromSchool.SchoolName ?? "Not Specified",
                ApprovedSchoolID = promotionRequest.ToSchoolIDApproved ?? null,
                PromotedToSchool = promotionRequest.ToSchoolApproved?.SchoolName ?? "Not Specified",
                RequestDate = promotionRequest.RequestDate,
                StatusID = promotionRequest.StatusID,
                Status = promotionRequest.Status?.StatusText ?? string.Empty,
                StatusChangeDate = promotionRequest.StatusChangeDate,
                RequestorCommand = promotionRequest.RequestorComment,
                ApproverCommand = promotionRequest.ApproverComment,
                FilePath = promotionRequest.FilePath,
                RequestedBy = promotionRequest.RequestedByID ,
                RequestedByUser = promotionRequest.RequestedByUser?.FirstName ?? string.Empty,
                ApprovedBy = promotionRequest.ApprovedByID ,
                ApprovedByUser = promotionRequest.ApprovedByUser?.FirstName ?? string.Empty

            };
        }
        public static void ToApprovePromotion(this Promotion PromotionRequest, Promotion updateRequest)
        {

            if (!string.IsNullOrWhiteSpace(updateRequest.ApproverComment))
                PromotionRequest.ApproverComment = updateRequest.ApproverComment;
            PromotionRequest.ToSchoolIDApproved = updateRequest.ToSchoolIDApproved;
           // Check if PromotionDate is a valid value before assigning
            if (updateRequest.PromotionDate != default(DateTime))
                PromotionRequest.PromotionDate = updateRequest.PromotionDate;


        }
        public static Promotion ToRejectPromotionRequest(this RejectPromotionRequestDTO rejectPromotionRequest)
        {
            return new Promotion
            {
                ApproverComment = rejectPromotionRequest.ApproverComment,
            };
        }

        public static TeacherPromotionEligibilityDTO GetEmployeePromotionListDTO(this Employee employee, List<Designation> allDesignations)
        {
            var currentDate = DateTime.Now;
            var nextDesignation = allDesignations
                .Where(d => d.DesignationID > employee.Designation.DesignationID)
                .OrderBy(d => d.DesignationID)
                .FirstOrDefault();

            // Get the names of qualified courses based on the designation's required qualifications
            var qualifiedCourses = employee.Designation.DesignationQualifications
                .Where(dq => employee.EmployeeEducations
                    .Any(edu => edu.CourseID == dq.QualificationID))
                .Select(dq => dq.Course.CourseName)
                .ToList();
            // Ensure PromotionRelinquishment is true only for the exact relinquishment year
            var hasPromotionRelinquishment = employee.PromotionRelinquishments
                .Any(pr => pr.yearOfRelinquishment.Year == currentDate.Year);


            return new TeacherPromotionEligibilityDTO
            {
                Id = employee.EmployeeID,
                Name = $"{employee.FirstName} {employee.LastName}",
                PhoneNumber = employee.Phone ?? "N/A",
                Subject = string.Join(", ", employee.Subject?.SubjectText ?? "No Subject Assigned"),
                ExperienceYear = CalculateExperience(employee.HireDate, currentDate),
                Age = CalculateExactAge(employee.DateOfBirth, currentDate),
                FromDesignation = employee.Designation.DesignationText ?? "No Designation",
                ToDesignation = nextDesignation?.DesignationText ?? "No Designation for promotion",
                SchoolName = employee.School?.SchoolName ?? "No School",
                QualifiedExam = qualifiedCourses.Any() ? string.Join(", ", qualifiedCourses) : "No Qualified Course",
                PromotionRelinquishment = hasPromotionRelinquishment,
                TeacherPopUpDTO = new TeacherPopUpDTO
                {
                    Photo = employee.Photo != null ? employee.Photo.PhotoImageName : null,
                    Name = $"{employee.FirstName ?? "Unknown"} {employee.LastName ?? "Unknown"}",
                    Subject = employee.Subject?.SubjectText ?? "No Subject Assigned",
                    SchoolName = employee.School != null
            ? $"{employee.School.SchoolName ?? "No School Name"}, " +
              $"{employee.School.Address ?? "No Address"}, " +
              $"{employee.School.City?.CityName ?? "No City Assigned"}, " +
              $"{employee.School.State ?? "No State Assigned"}, " +
              $"{employee.School.Pincode ?? "No Pincode"}"
            : "No School Assigned",
                    Email = employee.Email ?? "N/A",
                    PhoneNumber = employee.Phone ?? "N/A",
                    DateofJoin = employee.HireDate,
                    ReportedTo = employee.Supervisor != null
            ? $"{employee.Supervisor.FirstName ?? "Unknown"} {employee.Supervisor.LastName ?? "Unknown"}"
            : "N/A",
                }
            };
        }


        public static PromotionRelinquishment ToCreatePromotionRelinquishment(this CreatePromotionRelinquishmentDTO createPromotionRelinquishment)
        {
            var PromotionRelinquishment = new PromotionRelinquishment
            {
                EmployeeID = createPromotionRelinquishment.EmployeeID,
                yearOfRelinquishment = new DateTime(createPromotionRelinquishment.RelinquishmentYear, 1, 1), // Convert year to DateTime
                DocumentID = createPromotionRelinquishment.DocumentID
            };
            return PromotionRelinquishment;
        }




        public static GetPromotionRelinquishmentDTO ToGetPromotionRelinquishmentDTO(this PromotionRelinquishment relinquishment , List<Designation> allDesignations)
        {
            var nextDesignation = allDesignations
               .Where(d => d.DesignationID > relinquishment.Employee.Designation.DesignationID)
               .OrderBy(d => d.DesignationID)
               .FirstOrDefault();

            return new GetPromotionRelinquishmentDTO
            {
                RelinquishmentID = relinquishment.RelinquishmentID,
                EmployeeID = relinquishment.EmployeeID,
                EmployeeName = $"{relinquishment.Employee.FirstName} {relinquishment.Employee.LastName}",
                RelinquishmentYear = relinquishment.yearOfRelinquishment.Year,
                PromotedDesignation = nextDesignation?.DesignationText ?? "No Promotion Available", 
                DocumentID = relinquishment.DocumentID,
                DocumentFile = relinquishment.Document.DocumentFileName,
                ApprovalStatus = relinquishment.ApprovalStatus,
            };
        }

        public static PromotionRelinquishment ToApprovePromotionReliquishment(this ApprovePromotionRelinquishmentDTO update)
        {
            return new PromotionRelinquishment
            {
               ApprovalStatus = update.ApprovalStatus,
            };
        }


    }
}

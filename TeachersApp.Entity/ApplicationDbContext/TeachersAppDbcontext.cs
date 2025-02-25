using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersApp.Entity.Models;

namespace TeachersApp.Entity.ApplicationDbContext
{
    public class TeachersAppDbcontext : DbContext
    {
        public TeachersAppDbcontext(DbContextOptions<TeachersAppDbcontext> options)
            : base(options)
        {
        }
        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<TransferRequest> TransferRequests { get; set; }
        public DbSet<TeacherHistory> TeacherHistories { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<SchoolType> SchoolTypes { get; set; }
        public DbSet<SchoolPosition> SchoolPositions { get; set; }
        public DbSet<SchoolDivisionCount> SchoolDivisionCounts { get; set; }
        public DbSet<SchoolStandardType> SchoolStandardTypes { get; set; }
        public DbSet<SchoolTypeDesignation> SchoolTypeDesignations { get; set; }
        public DbSet<SchoolClass>SchoolClasses { get; set; }    
        public DbSet<School> Schools { get; set; }
        public DbSet<RolePrivilege> RolePrivileges { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<EmployeeReligion> EmployeeReligions { get; set; }
        public DbSet<EmployeePersonalDetails> EmployeePersonalDetails { get; set; }
        public DbSet<EmployeeMaritalStatus> employeeMaritalStatuses { get; set; }
        public DbSet<EmployeeCategory> EmployeeCategories { get; set; }
        public DbSet<EmployeeCasteCategory> EmployeeCasteCategories { get; set; }
        public DbSet<EmployeeBloodGroup> EmployeeBloodGroups { get; set; }
        public DbSet<EmployeeSex> EmployeeSexes { get; set; }
        public DbSet<EmployeeEducation> EmployeeEducations { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EducationType> EducationTypes { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<ChangeType> ChangeTypes { get; set; }
        public DbSet<ApprovalType> ApprovalTypes { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<DesignationQualification> DesignationQualifications { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
        public DbSet<PromotionRelinquishment> PromotionRelinquishments { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Entity
            modelBuilder.Entity<User>()
                    .HasKey(u => u.UserID);

            // Fluent API: Unique index on UniqueID
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.UniqueID)
                .IsUnique();

            // Fluent API: Unique index on PanID
            modelBuilder.Entity<EmployeePersonalDetails>()
                .HasIndex(e => e.PanID)
                .IsUnique();

            // Fluent API: Unique index on VoterID
            modelBuilder.Entity<EmployeePersonalDetails>()
                .HasIndex(e => e.VoterID)
                .IsUnique();

            // Fluent API: Unique index on VoterID
            modelBuilder.Entity<EmployeePersonalDetails>()
                .HasIndex(e => e.PEN)
                .IsUnique();

            modelBuilder.Entity<User>()
                  .HasOne(u => u.Role)
                  .WithMany(r => r.Users)
                  .HasForeignKey(u => u.RoleID)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                   .HasOne(u => u.Employee)
                   .WithOne(e => e.User) 
                   .HasForeignKey<User>(u => u.EmployeeID) 
                   .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<LeaveRequest>()
                   .HasOne(tr => tr.Employee)
                   .WithMany(e => e.LeaveRequests)
                   .HasForeignKey(tr => tr.EmployeeID)
                   .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LeaveRequest>()
                  .HasOne(tr => tr.Status)
                  .WithMany(s => s.LeaveRequests)
                  .HasForeignKey(tr => tr.StatusID)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LeaveRequest>()
                  .HasOne(tr => tr.RequestedByUser)
                  .WithMany(e => e.LeaveRequestedByUser)
                  .HasForeignKey(tr => tr.RequestedByID)
                  .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<LeaveRequest>()
                 .HasOne(tr => tr.ApprovedByUser)
                 .WithMany(e => e.LeaveApprovedByUser)
                 .HasForeignKey(tr => tr.ApprovedByID)
                 .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<LeaveRequest>()
              .HasOne(ee => ee.Document)
              .WithMany(d => d.LeaveRequests)
              .HasForeignKey(ee => ee.DocumentID)
              .OnDelete(DeleteBehavior.Restrict);
            //--
            modelBuilder.Entity<TransferRequest>()
                    .HasOne(tr => tr.Employee)
                    .WithMany(e => e.TransferRequests)
                    .HasForeignKey(tr => tr.EmployeeID)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TransferRequest>()
                  .HasOne(tr => tr.FromSchool)
                  .WithMany(s => s.TransferRequestsFrom)
                  .HasForeignKey(tr => tr.FromSchoolID)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransferRequest>()
                  .HasOne(tr => tr.ToSchool_One)
                  .WithMany(e => e.TransferRequestsToOne)
                  .HasForeignKey(tr => tr.ToSchoolID_One)
                  .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TransferRequest>()
                 .HasOne(tr => tr.ToSchool_Two)
                 .WithMany(e => e.TransferRequestsToTwo)
                 .HasForeignKey(tr => tr.ToSchoolID_Two)
                 .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TransferRequest>()
                 .HasOne(tr => tr.ToSchool_Three)
                 .WithMany(e => e.TransferRequestsToThree)
                 .HasForeignKey(tr => tr.ToSchoolID_Three)
                 .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TransferRequest>()
                 .HasOne(tr => tr.ToSchoolApproved)
                 .WithMany(e => e.TransferRequestsToApproved)
                 .HasForeignKey(tr => tr.ToSchoolIDApproved)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransferRequest>()
                  .HasOne(tr => tr.Status)
                  .WithMany(st => st.TransferRequests)
                  .HasForeignKey(tr => tr.StatusID)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransferRequest>()
                  .HasOne(tr => tr.RequestedByUser)
                  .WithMany(u => u.TransferRequestedByUser)
                  .HasForeignKey(tr => tr.RequestedByID)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransferRequest>()
                  .HasOne(tr => tr.ApprovedByUser)
                  .WithMany(st => st.TransferApprovedByUser)
                  .HasForeignKey(tr => tr.ApprovedByID)
                  .OnDelete(DeleteBehavior.Restrict);

           

            modelBuilder.Entity<TeacherHistory>()
              .HasOne(th => th.Employee)
              .WithMany(e => e.TeacherHistories)
              .HasForeignKey(th => th.EmployeeID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeacherHistory>()
              .HasOne(th => th.ChangeType)
              .WithMany(ct => ct.TeacherHistories)
              .HasForeignKey(th => th.ChangeTypeID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeacherHistory>()
                 .HasOne(th => th.SchoolFrom)
                 .WithMany(s => s.TeacherHistoriesFrom)
                 .HasForeignKey(th => th.ChangeFromSchoolID)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeacherHistory>()
                .HasOne(th => th.SchoolTo)
                .WithMany(s => s.TeacherHistoriesTo)
                .HasForeignKey(th => th.ChangeToSchoolID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeacherHistory>()
              .HasOne(th => th.PromotedFromDesignation)
              .WithMany(d => d.TeacherHistoryPromotionsFrom)
              .HasForeignKey(th => th.PromotedFromPositionID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeacherHistory>()
              .HasOne(th => th.PromotedToDesignation)
              .WithMany(d => d.TeacherHistoryPromotionsTo)
              .HasForeignKey(th => th.PromotedToPositionID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeacherHistory>()
              .HasOne(th => th.ChangedByUser)
              .WithMany(u => u.TeacherHistoryChangedByUser)
              .HasForeignKey(th => th.ChangedByID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SchoolPosition>()
              .HasOne(sp => sp.Designation)
              .WithMany(d => d.SchoolPositions)
              .HasForeignKey(sp => sp.DesignationID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SchoolPosition>()
              .HasOne(sp => sp.Subject)
              .WithMany(s => s.SchoolPositions)
              .HasForeignKey(sp => sp.SubjectID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SchoolPosition>()
              .HasOne(sp => sp.School)
              .WithMany(sb => sb.SchoolPositions)
              .HasForeignKey(sp => sp.SchoolID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SchoolPosition>()
              .HasOne(sp => sp.Status)
              .WithMany(st => st.SchoolPositions)
              .HasForeignKey(sp => sp.StatusID)
              .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<School>()
                .HasMany(s => s.SchoolClasses)
                .WithOne(sc => sc.School)
                .HasForeignKey(sc => sc.SchoolID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SchoolClass>()
                .HasMany(sc => sc.SchoolDivisionCounts)
                .WithOne(sd => sd.SchoolClass)
                .HasForeignKey(sd => sd.SchoolClassID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SchoolStandardType>()
              .HasOne(s => s.School)
              .WithMany(st => st.SchoolStandardTypes)
              .HasForeignKey(s => s.SchoolID)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SchoolStandardType>()
             .HasOne(s => s.SchoolType)
             .WithMany(ss => ss.SchoolStandardTypes)
             .HasForeignKey(st => st.SchoolTypeID)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SchoolTypeDesignation>()
             .HasOne(s => s.SchoolType)
             .WithMany(st => st.SchoolTypeDesignations)
             .HasForeignKey(s => s.SchoolTypeID)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PromotionRelinquishment>()
             .HasOne(s => s.Employee)
             .WithMany(ss => ss.PromotionRelinquishments)
             .HasForeignKey(st => st.EmployeeID)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PromotionRelinquishment>()
            .HasOne(s => s.Document)
            .WithMany(st => st.PromotionRelinquishments)
            .HasForeignKey(s => s.DocumentID)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SchoolTypeDesignation>()
             .HasOne(s => s.Designation)
             .WithMany(ss => ss.SchoolTypeDesignations)
             .HasForeignKey(st => st.DesignationID)
             .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<School>()
             .HasOne(s => s.City)
             .WithMany(ss => ss.Schools)
             .HasForeignKey(s => s.CityID)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<School>()
              .HasOne(s => s.Photo)
              .WithMany(st => st.Schools)
              .HasForeignKey(s => s.PhotoID)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<School>()
             .HasOne(s => s.Status)
             .WithMany(ss => ss.Schools)
             .HasForeignKey(s => s.StatusID)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<School>()
                .HasOne(s => s.Principal)
                .WithMany()
                .HasForeignKey(s => s.PrincipalID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<School>()
             .HasOne(s => s.VicePrincipal)
             .WithMany()
             .HasForeignKey(s => s.VicePrincipalID)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RolePrivilege>()
              .HasOne(rp => rp.Role)
              .WithMany(r => r.RolePrivileges)
              .HasForeignKey(rp => rp.RoleID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RolePrivilege>()
             .HasOne(rp => rp.Privilege)
             .WithMany(p => p.RolePrivileges)
             .HasForeignKey(rp => rp.PrivilegeID)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Promotion>()
              .HasOne(p => p.Employee)
              .WithMany(e => e.Promotions)
              .HasForeignKey(p => p.EmployeeID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Promotion>()
              .HasOne(p => p.PromotedFromDesignation)
              .WithMany(d => d.PromotionsFrom)
              .HasForeignKey(p => p.PromotedFromDesignationID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Promotion>()
              .HasOne(p => p.PromotedToDesignation)
              .WithMany(d => d.PromotionsTo)
              .HasForeignKey(p => p.PromotedToDesignationID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Promotion>()
              .HasOne(p => p.Status)
              .WithMany(st => st.Promotions)
              .HasForeignKey(p => p.StatusID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Promotion>()
              .HasOne(p => p.RequestedByUser)
              .WithMany(u => u.PromotionRequestedByUser)
              .HasForeignKey(p => p.RequestedByID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Promotion>()
              .HasOne(p => p.ApprovedByUser)
              .WithMany(u => u.PromotionApprovedByUser)
              .HasForeignKey(p => p.ApprovedByID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Promotion>()
                  .HasOne(tr => tr.FromSchool)
                  .WithMany(s => s.PromotionRequestsFromSchool)
                  .HasForeignKey(tr => tr.FromSchoolID)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Promotion>()
                  .HasOne(tr => tr.ToSchoolApproved)
                  .WithMany(s => s.PromotionRequestsToApprovedSchool)
                  .HasForeignKey(tr => tr.ToSchoolIDApproved)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeeType>()
              .HasMany(et => et.Employees)
              .WithOne(e => e.EmployeeType)
              .HasForeignKey(e => e.EmployeeTypeID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeeEducation>()
              .HasOne(ee => ee.Employee)
              .WithMany(e => e.EmployeeEducations)
              .HasForeignKey(ee => ee.EmployeeID)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeEducation>()
              .HasOne(ee => ee.Course)
              .WithMany(c => c.EmployeeEducations)
              .HasForeignKey(ee => ee.CourseID)
              .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<EmployeeEducation>()
              .HasOne(ee => ee.Document)
              .WithMany(d => d.EmployeeEducations)
              .HasForeignKey(ee => ee.DocumentID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeeDocument>()
              .HasOne(ee => ee.Employee)
              .WithMany(e => e.EmployeeDocuments)
              .HasForeignKey(ee => ee.EmployeeID)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeDocument>()
              .HasOne(ee => ee.Document)
              .WithMany(d => d.EmployeeDocuments)
              .HasForeignKey(ee => ee.DocumentID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
              .HasOne(e => e.EmployeeType)
              .WithMany(et => et.Employees)
              .HasForeignKey(e => e.EmployeeTypeID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
             .HasOne(e => e.Photo)
             .WithMany(p => p.Employees)
             .HasForeignKey(e => e.PhotoID)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
             .HasOne(e => e.Designation)
             .WithMany(d => d.Employees)
             .HasForeignKey(e => e.DesignationID)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
             .HasOne(e => e.Subject)
             .WithMany(d => d.Employees)
             .HasForeignKey(e => e.SubjectID)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
             .HasOne(e => e.School)
             .WithMany(s => s.Employees)
             .HasForeignKey(e => e.SchoolID)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
             .HasOne(e => e.Status)
             .WithMany(st => st.Employees)
             .HasForeignKey(e => e.StatusID)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
             .HasOne(e => e.ApprovalType)
             .WithMany(at => at.Employees)
             .HasForeignKey(e => e.ApprovalTypeID)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
             .HasOne(e => e.Supervisor)
             .WithMany(e => e.Employees)
             .HasForeignKey(e => e.SupervisorID)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
             .HasOne(e => e.SchoolsPosition)
             .WithMany(sp => sp.EmployeesPosition)
             .HasForeignKey(e => e.SchoolPositionID)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
            .HasOne(e => e.EmployeeCategory)
            .WithMany(sp => sp.Employees)
            .HasForeignKey(e => e.CategoryID)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.PersonalDetails)
                .WithOne(p => p.Employee)
                .HasForeignKey<EmployeePersonalDetails>(p => p.EmployeeID)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeePersonalDetails>()
                 .HasOne(epd => epd.Sex)
                 .WithMany(es => es.PersonalDetails)
                 .HasForeignKey(epd => epd.SexID)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeePersonalDetails>()
                .HasOne(epd => epd.EmployeeReligion)
                .WithMany(er => er.PersonalDetailsReligionID)
                .HasForeignKey(epd => epd.ReligionID)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeePersonalDetails>()
                .HasOne(epd => epd.CasteCategory)
                .WithMany(cc => cc.PersonalDetails)
                .HasForeignKey(epd => epd.CasteID)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeePersonalDetails>()
                .HasOne(epd => epd.BloodGroup)
                .WithMany(bg => bg.PersonalDetails)
                .HasForeignKey(epd => epd.BloodGroupID)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EmployeePersonalDetails>()
                .HasOne(epd => epd.MaritalStatus)
                .WithMany(ms => ms.PersonalDetails)
                .HasForeignKey(epd => epd.MaritalStatusID)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<EmployeePersonalDetails>()
               .HasOne(epd => epd.District)
               .WithMany(ms => ms.PersonalDetails)
               .HasForeignKey(epd => epd.DistrictID)
                .OnDelete(DeleteBehavior.Restrict);


            // Configuring One-to-Many Relationship
            modelBuilder.Entity<EmployeePersonalDetails>()
                .HasOne(epd => epd.EmployeeSpouseReligion)                
                .WithMany(er => er.SpousePersonalDetailsReligionID)    
                .HasForeignKey(epd => epd.SpouseReligionID)          
                .OnDelete(DeleteBehavior.Restrict);            


            modelBuilder.Entity<Document>()
              .HasOne(d => d.Status)
              .WithMany(ss => ss.Documents)
              .HasForeignKey(d => d.StatusID)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
              .HasOne(c => c.EducationType)
              .WithMany(et => et.Courses)
              .HasForeignKey(c => c.EducationTypeID)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DesignationQualification>()
             .HasOne(dq => dq.Designation)
             .WithMany(d => d.DesignationQualifications)
             .HasForeignKey(dq => dq.DesignationID)
             .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<DesignationQualification>()
             .HasOne(dq => dq.Course)
             .WithMany(d => d.DesignationQualifications)
             .HasForeignKey(dq => dq.QualificationID)
             .OnDelete(DeleteBehavior.Cascade);

        }
    }
}

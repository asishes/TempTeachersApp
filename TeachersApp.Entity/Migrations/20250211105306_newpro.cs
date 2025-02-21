using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeachersApp.Entity.Migrations
{
    /// <inheritdoc />
    public partial class newpro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordResetTokenExpiry",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserRole",
                table: "Users",
                newName: "RoleID");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "UserID");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EmployeeID",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ApprovalTypes",
                columns: table => new
                {
                    ApprovalTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Approvaltype = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalTypes", x => x.ApprovalTypeID);
                });

            migrationBuilder.CreateTable(
                name: "ChangeTypes",
                columns: table => new
                {
                    ChangeTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangeText = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeTypes", x => x.ChangeTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    CityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityID);
                });

            migrationBuilder.CreateTable(
                name: "Designations",
                columns: table => new
                {
                    DesignationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DesignationText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designations", x => x.DesignationID);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    DistrictID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistrictName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.DistrictID);
                });

            migrationBuilder.CreateTable(
                name: "EducationTypes",
                columns: table => new
                {
                    EducationTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EductionTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationTypes", x => x.EducationTypeID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeBloodGroups",
                columns: table => new
                {
                    BloodGroupID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BloodGroupName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeBloodGroups", x => x.BloodGroupID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeCasteCategories",
                columns: table => new
                {
                    CasteCategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CasteCategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCasteCategories", x => x.CasteCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeCategories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCategories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "employeeMaritalStatuses",
                columns: table => new
                {
                    MaritalStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaritalStatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employeeMaritalStatuses", x => x.MaritalStatusID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeReligions",
                columns: table => new
                {
                    ReligionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReligionName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeReligions", x => x.ReligionID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSexes",
                columns: table => new
                {
                    SexID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sex = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSexes", x => x.SexID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTypes",
                columns: table => new
                {
                    EmployeeTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Employeetype = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTypes", x => x.EmployeeTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    PhotoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhotoImageName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.PhotoID);
                });

            migrationBuilder.CreateTable(
                name: "Privileges",
                columns: table => new
                {
                    PrivilegeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrivilegeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privileges", x => x.PrivilegeID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "SchoolTypes",
                columns: table => new
                {
                    SchoolTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Class = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolTypes", x => x.SchoolTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusText = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SubjectID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SubjectID);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EducationTypeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseID);
                    table.ForeignKey(
                        name: "FK_Courses_EducationTypes_EducationTypeID",
                        column: x => x.EducationTypeID,
                        principalTable: "EducationTypes",
                        principalColumn: "EducationTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePrivileges",
                columns: table => new
                {
                    RolePrivilegeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    PrivilegeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePrivileges", x => x.RolePrivilegeID);
                    table.ForeignKey(
                        name: "FK_RolePrivileges_Privileges_PrivilegeID",
                        column: x => x.PrivilegeID,
                        principalTable: "Privileges",
                        principalColumn: "PrivilegeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePrivileges_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchoolTypeDesignations",
                columns: table => new
                {
                    SchoolTypeDesignationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolTypeID = table.Column<int>(type: "int", nullable: false),
                    DesignationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolTypeDesignations", x => x.SchoolTypeDesignationID);
                    table.ForeignKey(
                        name: "FK_SchoolTypeDesignations_Designations_DesignationID",
                        column: x => x.DesignationID,
                        principalTable: "Designations",
                        principalColumn: "DesignationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolTypeDesignations_SchoolTypes_SchoolTypeID",
                        column: x => x.SchoolTypeID,
                        principalTable: "SchoolTypes",
                        principalColumn: "SchoolTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentText = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DocumentFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentID);
                    table.ForeignKey(
                        name: "FK_Documents_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DesignationQualifications",
                columns: table => new
                {
                    DesQuaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DesignationID = table.Column<int>(type: "int", nullable: false),
                    QualificationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignationQualifications", x => x.DesQuaID);
                    table.ForeignKey(
                        name: "FK_DesignationQualifications_Courses_QualificationID",
                        column: x => x.QualificationID,
                        principalTable: "Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DesignationQualifications_Designations_DesignationID",
                        column: x => x.DesignationID,
                        principalTable: "Designations",
                        principalColumn: "DesignationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeDocuments",
                columns: table => new
                {
                    EmployeeDocumentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: true),
                    DocumentID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDocuments", x => x.EmployeeDocumentID);
                    table.ForeignKey(
                        name: "FK_EmployeeDocuments_Documents_DocumentID",
                        column: x => x.DocumentID,
                        principalTable: "Documents",
                        principalColumn: "DocumentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeEducations",
                columns: table => new
                {
                    EmployeecourseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: true),
                    CourseID = table.Column<int>(type: "int", nullable: true),
                    CourseName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    University = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocumentID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEducations", x => x.EmployeecourseID);
                    table.ForeignKey(
                        name: "FK_EmployeeEducations_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeEducations_Documents_DocumentID",
                        column: x => x.DocumentID,
                        principalTable: "Documents",
                        principalColumn: "DocumentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeePersonalDetails",
                columns: table => new
                {
                    EmployeeDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: true),
                    SexID = table.Column<int>(type: "int", nullable: true),
                    ReligionID = table.Column<int>(type: "int", nullable: true),
                    Caste = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CasteID = table.Column<int>(type: "int", nullable: true),
                    BloodGroupID = table.Column<int>(type: "int", nullable: true),
                    DistrictID = table.Column<int>(type: "int", nullable: true),
                    DifferentlyAbled = table.Column<bool>(type: "bit", nullable: true),
                    ExServiceMen = table.Column<bool>(type: "bit", nullable: true),
                    IdentificationMark1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IdentificationMark2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Height = table.Column<double>(type: "float", nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MotherName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InterReligion = table.Column<bool>(type: "bit", nullable: true),
                    MaritalStatusID = table.Column<int>(type: "int", nullable: true),
                    SpouseName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SpouseReligionID = table.Column<int>(type: "int", nullable: true),
                    SpouseCaste = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PanID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VoterID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AadhaarID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PFNummber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PRAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PEN = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EligibilityTestQualified = table.Column<bool>(type: "bit", nullable: true),
                    ProtectedTeacher = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePersonalDetails", x => x.EmployeeDetailID);
                    table.ForeignKey(
                        name: "FK_EmployeePersonalDetails_Districts_DistrictID",
                        column: x => x.DistrictID,
                        principalTable: "Districts",
                        principalColumn: "DistrictID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeePersonalDetails_EmployeeBloodGroups_BloodGroupID",
                        column: x => x.BloodGroupID,
                        principalTable: "EmployeeBloodGroups",
                        principalColumn: "BloodGroupID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeePersonalDetails_EmployeeCasteCategories_CasteID",
                        column: x => x.CasteID,
                        principalTable: "EmployeeCasteCategories",
                        principalColumn: "CasteCategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeePersonalDetails_EmployeeReligions_ReligionID",
                        column: x => x.ReligionID,
                        principalTable: "EmployeeReligions",
                        principalColumn: "ReligionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeePersonalDetails_EmployeeReligions_SpouseReligionID",
                        column: x => x.SpouseReligionID,
                        principalTable: "EmployeeReligions",
                        principalColumn: "ReligionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeePersonalDetails_EmployeeSexes_SexID",
                        column: x => x.SexID,
                        principalTable: "EmployeeSexes",
                        principalColumn: "SexID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeePersonalDetails_employeeMaritalStatuses_MaritalStatusID",
                        column: x => x.MaritalStatusID,
                        principalTable: "employeeMaritalStatuses",
                        principalColumn: "MaritalStatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmployeeTypeID = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    PresentAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PermanentAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhotoID = table.Column<int>(type: "int", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WorkStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RetirementDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DesignationID = table.Column<int>(type: "int", nullable: true),
                    CategoryID = table.Column<int>(type: "int", nullable: true),
                    SchoolID = table.Column<int>(type: "int", nullable: true),
                    SubjectID = table.Column<int>(type: "int", nullable: true),
                    SchoolPositionID = table.Column<int>(type: "int", nullable: true),
                    StatusID = table.Column<int>(type: "int", nullable: true),
                    ApprovalTypeID = table.Column<int>(type: "int", nullable: true),
                    ApprovalTypeReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupervisorID = table.Column<int>(type: "int", nullable: true),
                    PromotionEligible = table.Column<bool>(type: "bit", nullable: false),
                    PromotionSeniorityNumber = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_Employees_ApprovalTypes_ApprovalTypeID",
                        column: x => x.ApprovalTypeID,
                        principalTable: "ApprovalTypes",
                        principalColumn: "ApprovalTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Designations_DesignationID",
                        column: x => x.DesignationID,
                        principalTable: "Designations",
                        principalColumn: "DesignationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_EmployeeCategories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "EmployeeCategories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_EmployeeTypes_EmployeeTypeID",
                        column: x => x.EmployeeTypeID,
                        principalTable: "EmployeeTypes",
                        principalColumn: "EmployeeTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_SupervisorID",
                        column: x => x.SupervisorID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Photos_PhotoID",
                        column: x => x.PhotoID,
                        principalTable: "Photos",
                        principalColumn: "PhotoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Subjects_SubjectID",
                        column: x => x.SubjectID,
                        principalTable: "Subjects",
                        principalColumn: "SubjectID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaveRequests",
                columns: table => new
                {
                    LeaveRequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    StatusChangeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequestedByID = table.Column<int>(type: "int", nullable: true),
                    ApprovedByID = table.Column<int>(type: "int", nullable: true),
                    RequestorComment = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ApproverComment = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DocumentID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequests", x => x.LeaveRequestID);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Documents_DocumentID",
                        column: x => x.DocumentID,
                        principalTable: "Documents",
                        principalColumn: "DocumentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Users_ApprovedByID",
                        column: x => x.ApprovedByID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Users_RequestedByID",
                        column: x => x.RequestedByID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PromotionRelinquishments",
                columns: table => new
                {
                    RelinquishmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    yearOfRelinquishment = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentID = table.Column<int>(type: "int", nullable: true),
                    ApprovalStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionRelinquishments", x => x.RelinquishmentID);
                    table.ForeignKey(
                        name: "FK_PromotionRelinquishments_Documents_DocumentID",
                        column: x => x.DocumentID,
                        principalTable: "Documents",
                        principalColumn: "DocumentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PromotionRelinquishments_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    SchoolID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CityID = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Pincode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    PhotoID = table.Column<int>(type: "int", nullable: true),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    PrincipalID = table.Column<int>(type: "int", nullable: true),
                    VicePrincipalID = table.Column<int>(type: "int", nullable: true),
                    EmployeeID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.SchoolID);
                    table.ForeignKey(
                        name: "FK_Schools_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "CityID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Schools_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_Schools_Employees_PrincipalID",
                        column: x => x.PrincipalID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Schools_Employees_VicePrincipalID",
                        column: x => x.VicePrincipalID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Schools_Photos_PhotoID",
                        column: x => x.PhotoID,
                        principalTable: "Photos",
                        principalColumn: "PhotoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schools_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    PromotionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    FromSchoolID = table.Column<int>(type: "int", nullable: true),
                    ToSchoolIDApproved = table.Column<int>(type: "int", nullable: true),
                    PromotionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PromotedFromDesignationID = table.Column<int>(type: "int", nullable: false),
                    PromotedToDesignationID = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    StatusChangeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequestedByID = table.Column<int>(type: "int", nullable: true),
                    ApprovedByID = table.Column<int>(type: "int", nullable: true),
                    RequestorComment = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ApproverComment = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.PromotionID);
                    table.ForeignKey(
                        name: "FK_Promotions_Designations_PromotedFromDesignationID",
                        column: x => x.PromotedFromDesignationID,
                        principalTable: "Designations",
                        principalColumn: "DesignationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Promotions_Designations_PromotedToDesignationID",
                        column: x => x.PromotedToDesignationID,
                        principalTable: "Designations",
                        principalColumn: "DesignationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Promotions_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Promotions_Schools_FromSchoolID",
                        column: x => x.FromSchoolID,
                        principalTable: "Schools",
                        principalColumn: "SchoolID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Promotions_Schools_ToSchoolIDApproved",
                        column: x => x.ToSchoolIDApproved,
                        principalTable: "Schools",
                        principalColumn: "SchoolID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Promotions_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Promotions_Users_ApprovedByID",
                        column: x => x.ApprovedByID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Promotions_Users_RequestedByID",
                        column: x => x.RequestedByID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchoolClasses",
                columns: table => new
                {
                    SchoolClassID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolID = table.Column<int>(type: "int", nullable: false),
                    Class = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolClasses", x => x.SchoolClassID);
                    table.ForeignKey(
                        name: "FK_SchoolClasses_Schools_SchoolID",
                        column: x => x.SchoolID,
                        principalTable: "Schools",
                        principalColumn: "SchoolID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolPositions",
                columns: table => new
                {
                    PositionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DesignationID = table.Column<int>(type: "int", nullable: false),
                    SubjectID = table.Column<int>(type: "int", nullable: false),
                    SchoolID = table.Column<int>(type: "int", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolPositions", x => x.PositionID);
                    table.ForeignKey(
                        name: "FK_SchoolPositions_Designations_DesignationID",
                        column: x => x.DesignationID,
                        principalTable: "Designations",
                        principalColumn: "DesignationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolPositions_Schools_SchoolID",
                        column: x => x.SchoolID,
                        principalTable: "Schools",
                        principalColumn: "SchoolID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolPositions_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolPositions_Subjects_SubjectID",
                        column: x => x.SubjectID,
                        principalTable: "Subjects",
                        principalColumn: "SubjectID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchoolStandardTypes",
                columns: table => new
                {
                    SchoolStandardTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolID = table.Column<int>(type: "int", nullable: false),
                    SchoolTypeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolStandardTypes", x => x.SchoolStandardTypeID);
                    table.ForeignKey(
                        name: "FK_SchoolStandardTypes_SchoolTypes_SchoolTypeID",
                        column: x => x.SchoolTypeID,
                        principalTable: "SchoolTypes",
                        principalColumn: "SchoolTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolStandardTypes_Schools_SchoolID",
                        column: x => x.SchoolID,
                        principalTable: "Schools",
                        principalColumn: "SchoolID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherHistories",
                columns: table => new
                {
                    HistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedByID = table.Column<int>(type: "int", nullable: true),
                    ChangeDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangeTypeID = table.Column<int>(type: "int", nullable: true),
                    ChangeFromSchoolID = table.Column<int>(type: "int", nullable: true),
                    ChangeToSchoolID = table.Column<int>(type: "int", nullable: true),
                    PromotedFromPositionID = table.Column<int>(type: "int", nullable: true),
                    PromotedToPositionID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherHistories", x => x.HistoryID);
                    table.ForeignKey(
                        name: "FK_TeacherHistories_ChangeTypes_ChangeTypeID",
                        column: x => x.ChangeTypeID,
                        principalTable: "ChangeTypes",
                        principalColumn: "ChangeTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherHistories_Designations_PromotedFromPositionID",
                        column: x => x.PromotedFromPositionID,
                        principalTable: "Designations",
                        principalColumn: "DesignationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherHistories_Designations_PromotedToPositionID",
                        column: x => x.PromotedToPositionID,
                        principalTable: "Designations",
                        principalColumn: "DesignationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherHistories_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherHistories_Schools_ChangeFromSchoolID",
                        column: x => x.ChangeFromSchoolID,
                        principalTable: "Schools",
                        principalColumn: "SchoolID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherHistories_Schools_ChangeToSchoolID",
                        column: x => x.ChangeToSchoolID,
                        principalTable: "Schools",
                        principalColumn: "SchoolID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherHistories_Users_ChangedByID",
                        column: x => x.ChangedByID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferRequests",
                columns: table => new
                {
                    TransferRequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    FromSchoolID = table.Column<int>(type: "int", nullable: false),
                    ToSchoolID_One = table.Column<int>(type: "int", nullable: false),
                    ToSchoolID_Two = table.Column<int>(type: "int", nullable: false),
                    ToSchoolID_Three = table.Column<int>(type: "int", nullable: false),
                    ToSchoolIDApproved = table.Column<int>(type: "int", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    StatusChangeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TransferDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequestedByID = table.Column<int>(type: "int", nullable: true),
                    ApprovedByID = table.Column<int>(type: "int", nullable: true),
                    RequestorComment = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ApproverComment = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferRequests", x => x.TransferRequestID);
                    table.ForeignKey(
                        name: "FK_TransferRequests_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransferRequests_Schools_FromSchoolID",
                        column: x => x.FromSchoolID,
                        principalTable: "Schools",
                        principalColumn: "SchoolID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferRequests_Schools_ToSchoolIDApproved",
                        column: x => x.ToSchoolIDApproved,
                        principalTable: "Schools",
                        principalColumn: "SchoolID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferRequests_Schools_ToSchoolID_One",
                        column: x => x.ToSchoolID_One,
                        principalTable: "Schools",
                        principalColumn: "SchoolID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferRequests_Schools_ToSchoolID_Three",
                        column: x => x.ToSchoolID_Three,
                        principalTable: "Schools",
                        principalColumn: "SchoolID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferRequests_Schools_ToSchoolID_Two",
                        column: x => x.ToSchoolID_Two,
                        principalTable: "Schools",
                        principalColumn: "SchoolID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferRequests_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferRequests_Users_ApprovedByID",
                        column: x => x.ApprovedByID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferRequests_Users_RequestedByID",
                        column: x => x.RequestedByID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchoolDivisionCounts",
                columns: table => new
                {
                    DivisionCountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolClassID = table.Column<int>(type: "int", nullable: false),
                    Division = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolDivisionCounts", x => x.DivisionCountID);
                    table.ForeignKey(
                        name: "FK_SchoolDivisionCounts_SchoolClasses_SchoolClassID",
                        column: x => x.SchoolClassID,
                        principalTable: "SchoolClasses",
                        principalColumn: "SchoolClassID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployeeID",
                table: "Users",
                column: "EmployeeID",
                unique: true,
                filter: "[EmployeeID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_EducationTypeID",
                table: "Courses",
                column: "EducationTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_DesignationQualifications_DesignationID",
                table: "DesignationQualifications",
                column: "DesignationID");

            migrationBuilder.CreateIndex(
                name: "IX_DesignationQualifications_QualificationID",
                table: "DesignationQualifications",
                column: "QualificationID");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_StatusID",
                table: "Documents",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocuments_DocumentID",
                table: "EmployeeDocuments",
                column: "DocumentID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocuments_EmployeeID",
                table: "EmployeeDocuments",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_CourseID",
                table: "EmployeeEducations",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_DocumentID",
                table: "EmployeeEducations",
                column: "DocumentID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEducations_EmployeeID",
                table: "EmployeeEducations",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePersonalDetails_BloodGroupID",
                table: "EmployeePersonalDetails",
                column: "BloodGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePersonalDetails_CasteID",
                table: "EmployeePersonalDetails",
                column: "CasteID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePersonalDetails_DistrictID",
                table: "EmployeePersonalDetails",
                column: "DistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePersonalDetails_EmployeeID",
                table: "EmployeePersonalDetails",
                column: "EmployeeID",
                unique: true,
                filter: "[EmployeeID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePersonalDetails_MaritalStatusID",
                table: "EmployeePersonalDetails",
                column: "MaritalStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePersonalDetails_PanID",
                table: "EmployeePersonalDetails",
                column: "PanID",
                unique: true,
                filter: "[PanID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePersonalDetails_PEN",
                table: "EmployeePersonalDetails",
                column: "PEN",
                unique: true,
                filter: "[PEN] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePersonalDetails_ReligionID",
                table: "EmployeePersonalDetails",
                column: "ReligionID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePersonalDetails_SexID",
                table: "EmployeePersonalDetails",
                column: "SexID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePersonalDetails_SpouseReligionID",
                table: "EmployeePersonalDetails",
                column: "SpouseReligionID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePersonalDetails_VoterID",
                table: "EmployeePersonalDetails",
                column: "VoterID",
                unique: true,
                filter: "[VoterID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ApprovalTypeID",
                table: "Employees",
                column: "ApprovalTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CategoryID",
                table: "Employees",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DesignationID",
                table: "Employees",
                column: "DesignationID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeTypeID",
                table: "Employees",
                column: "EmployeeTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PhotoID",
                table: "Employees",
                column: "PhotoID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SchoolID",
                table: "Employees",
                column: "SchoolID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SchoolPositionID",
                table: "Employees",
                column: "SchoolPositionID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_StatusID",
                table: "Employees",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SubjectID",
                table: "Employees",
                column: "SubjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SupervisorID",
                table: "Employees",
                column: "SupervisorID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UniqueID",
                table: "Employees",
                column: "UniqueID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_ApprovedByID",
                table: "LeaveRequests",
                column: "ApprovedByID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_DocumentID",
                table: "LeaveRequests",
                column: "DocumentID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_EmployeeID",
                table: "LeaveRequests",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_RequestedByID",
                table: "LeaveRequests",
                column: "RequestedByID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_StatusID",
                table: "LeaveRequests",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionRelinquishments_DocumentID",
                table: "PromotionRelinquishments",
                column: "DocumentID");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionRelinquishments_EmployeeID",
                table: "PromotionRelinquishments",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_ApprovedByID",
                table: "Promotions",
                column: "ApprovedByID");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_EmployeeID",
                table: "Promotions",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_FromSchoolID",
                table: "Promotions",
                column: "FromSchoolID");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_PromotedFromDesignationID",
                table: "Promotions",
                column: "PromotedFromDesignationID");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_PromotedToDesignationID",
                table: "Promotions",
                column: "PromotedToDesignationID");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_RequestedByID",
                table: "Promotions",
                column: "RequestedByID");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_StatusID",
                table: "Promotions",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_ToSchoolIDApproved",
                table: "Promotions",
                column: "ToSchoolIDApproved");

            migrationBuilder.CreateIndex(
                name: "IX_RolePrivileges_PrivilegeID",
                table: "RolePrivileges",
                column: "PrivilegeID");

            migrationBuilder.CreateIndex(
                name: "IX_RolePrivileges_RoleID",
                table: "RolePrivileges",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolClasses_SchoolID",
                table: "SchoolClasses",
                column: "SchoolID");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolDivisionCounts_SchoolClassID",
                table: "SchoolDivisionCounts",
                column: "SchoolClassID");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolPositions_DesignationID",
                table: "SchoolPositions",
                column: "DesignationID");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolPositions_SchoolID",
                table: "SchoolPositions",
                column: "SchoolID");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolPositions_StatusID",
                table: "SchoolPositions",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolPositions_SubjectID",
                table: "SchoolPositions",
                column: "SubjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_CityID",
                table: "Schools",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_EmployeeID",
                table: "Schools",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_PhotoID",
                table: "Schools",
                column: "PhotoID");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_PrincipalID",
                table: "Schools",
                column: "PrincipalID");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_StatusID",
                table: "Schools",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_VicePrincipalID",
                table: "Schools",
                column: "VicePrincipalID");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolStandardTypes_SchoolID",
                table: "SchoolStandardTypes",
                column: "SchoolID");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolStandardTypes_SchoolTypeID",
                table: "SchoolStandardTypes",
                column: "SchoolTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTypeDesignations_DesignationID",
                table: "SchoolTypeDesignations",
                column: "DesignationID");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTypeDesignations_SchoolTypeID",
                table: "SchoolTypeDesignations",
                column: "SchoolTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherHistories_ChangedByID",
                table: "TeacherHistories",
                column: "ChangedByID");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherHistories_ChangeFromSchoolID",
                table: "TeacherHistories",
                column: "ChangeFromSchoolID");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherHistories_ChangeToSchoolID",
                table: "TeacherHistories",
                column: "ChangeToSchoolID");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherHistories_ChangeTypeID",
                table: "TeacherHistories",
                column: "ChangeTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherHistories_EmployeeID",
                table: "TeacherHistories",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherHistories_PromotedFromPositionID",
                table: "TeacherHistories",
                column: "PromotedFromPositionID");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherHistories_PromotedToPositionID",
                table: "TeacherHistories",
                column: "PromotedToPositionID");

            migrationBuilder.CreateIndex(
                name: "IX_TransferRequests_ApprovedByID",
                table: "TransferRequests",
                column: "ApprovedByID");

            migrationBuilder.CreateIndex(
                name: "IX_TransferRequests_EmployeeID",
                table: "TransferRequests",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_TransferRequests_FromSchoolID",
                table: "TransferRequests",
                column: "FromSchoolID");

            migrationBuilder.CreateIndex(
                name: "IX_TransferRequests_RequestedByID",
                table: "TransferRequests",
                column: "RequestedByID");

            migrationBuilder.CreateIndex(
                name: "IX_TransferRequests_StatusID",
                table: "TransferRequests",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_TransferRequests_ToSchoolID_One",
                table: "TransferRequests",
                column: "ToSchoolID_One");

            migrationBuilder.CreateIndex(
                name: "IX_TransferRequests_ToSchoolID_Three",
                table: "TransferRequests",
                column: "ToSchoolID_Three");

            migrationBuilder.CreateIndex(
                name: "IX_TransferRequests_ToSchoolID_Two",
                table: "TransferRequests",
                column: "ToSchoolID_Two");

            migrationBuilder.CreateIndex(
                name: "IX_TransferRequests_ToSchoolIDApproved",
                table: "TransferRequests",
                column: "ToSchoolIDApproved");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Employees_EmployeeID",
                table: "Users",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleID",
                table: "Users",
                column: "RoleID",
                principalTable: "Roles",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeDocuments_Employees_EmployeeID",
                table: "EmployeeDocuments",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeEducations_Employees_EmployeeID",
                table: "EmployeeEducations",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeePersonalDetails_Employees_EmployeeID",
                table: "EmployeePersonalDetails",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_SchoolPositions_SchoolPositionID",
                table: "Employees",
                column: "SchoolPositionID",
                principalTable: "SchoolPositions",
                principalColumn: "PositionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Schools_SchoolID",
                table: "Employees",
                column: "SchoolID",
                principalTable: "Schools",
                principalColumn: "SchoolID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Employees_EmployeeID",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleID",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Designations_DesignationID",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolPositions_Designations_DesignationID",
                table: "SchoolPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Statuses_StatusID",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolPositions_Statuses_StatusID",
                table: "SchoolPositions");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_Statuses_StatusID",
                table: "Schools");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_Employees_EmployeeID",
                table: "Schools");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_Employees_PrincipalID",
                table: "Schools");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_Employees_VicePrincipalID",
                table: "Schools");

            migrationBuilder.DropTable(
                name: "DesignationQualifications");

            migrationBuilder.DropTable(
                name: "EmployeeDocuments");

            migrationBuilder.DropTable(
                name: "EmployeeEducations");

            migrationBuilder.DropTable(
                name: "EmployeePersonalDetails");

            migrationBuilder.DropTable(
                name: "LeaveRequests");

            migrationBuilder.DropTable(
                name: "PromotionRelinquishments");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropTable(
                name: "RolePrivileges");

            migrationBuilder.DropTable(
                name: "SchoolDivisionCounts");

            migrationBuilder.DropTable(
                name: "SchoolStandardTypes");

            migrationBuilder.DropTable(
                name: "SchoolTypeDesignations");

            migrationBuilder.DropTable(
                name: "TeacherHistories");

            migrationBuilder.DropTable(
                name: "TransferRequests");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "EmployeeBloodGroups");

            migrationBuilder.DropTable(
                name: "EmployeeCasteCategories");

            migrationBuilder.DropTable(
                name: "EmployeeReligions");

            migrationBuilder.DropTable(
                name: "EmployeeSexes");

            migrationBuilder.DropTable(
                name: "employeeMaritalStatuses");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Privileges");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "SchoolClasses");

            migrationBuilder.DropTable(
                name: "SchoolTypes");

            migrationBuilder.DropTable(
                name: "ChangeTypes");

            migrationBuilder.DropTable(
                name: "EducationTypes");

            migrationBuilder.DropTable(
                name: "Designations");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "ApprovalTypes");

            migrationBuilder.DropTable(
                name: "EmployeeCategories");

            migrationBuilder.DropTable(
                name: "EmployeeTypes");

            migrationBuilder.DropTable(
                name: "SchoolPositions");

            migrationBuilder.DropTable(
                name: "Schools");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Users_EmployeeID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmployeeID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "RoleID",
                table: "Users",
                newName: "UserRole");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Users",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Users",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Users",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "Users",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetTokenExpiry",
                table: "Users",
                type: "datetime",
                nullable: true);
        }
    }
}

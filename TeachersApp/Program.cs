using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TeachersApp.Entity.ApplicationDbContext;
using TeachersApp.Entity.Helpers;
using TeachersApp.Entity.Models;
using TeachersApp.Services.FileServices;
using TeachersApp.Services.Interfaces;
using TeachersApp.Services.Repositories;
using TeachersApp.Services.SheduleServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Db Configs
builder.Services.AddDbContext<TeachersAppDbcontext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Hangfire configuration
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddHangfireServer();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()  // Allows any origin
               .AllowAnyMethod()  // Allows any HTTP method
               .AllowAnyHeader(); // Allows any header
    });
});

var emailConfig = builder.Configuration
    .GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);


// Scoping

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<ISchoolService, SchoolService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IPersonalDetailsService, PersonalDetailsService>();
builder.Services.AddScoped<ISchoolTypeService, SchoolTypeService>();
builder.Services.AddScoped<ISchoolDivisionService, SchoolDivisionService>();
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<IFileService,FileService>();
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddHostedService<ShedulerService>(); 
builder.Services.AddScoped<INonTeacherInterface, NonTeacherService>();
builder.Services.AddScoped<ITransferRequestService, TransferRequestService>();
builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
builder.Services.AddScoped<ITeacherHistoryService, TeacherHistoryService>();



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddControllers();

// Swagger/OpenAPI configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TeachersApp API", Version = "v1" });

    // Define the security scheme for JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TeachersApp API v1");
    });
}

// mapping Uploads folder to folder
var uploadsPath = Path.Combine(builder.Environment.ContentRootPath, "Uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/Uploads"
});
// Use Hangfire Dashboard (optional, for viewing scheduled jobs)
app.UseHangfireDashboard("/hangfire"); // This sets up the Hangfire dashboard at /hangfire endpoint

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication(); 

app.UseAuthorization();

app.MapControllers();

app.Run();

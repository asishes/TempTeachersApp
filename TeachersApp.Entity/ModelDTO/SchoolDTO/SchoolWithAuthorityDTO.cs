using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.SchoolDTO
{
    public class SchoolWithAuthorityDTO
    {
        public int SchoolID { get; set; }   
        public string SchoolName { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty ;
        public string? PrincipalName { get; set; } = string.Empty;
        public string? Email {  get; set; } = string.Empty ;
        public string? Address { get; set; } = string.Empty;
        public string? PrincipalPhone { get; set; } = string.Empty;
        public string? VicePrincipalName { get; set; } = string.Empty;
        public string? VicePrincipalPhone { get; set; } = string.Empty;
    }
}

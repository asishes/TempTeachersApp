using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.TransferRequestDTO
{
    public class CreateTransferRequestDTO
    {
        public int EmployeeID { get; set; }

        public int ToSchoolIDOne { get;set; }
        public int ToSchoolIDTwo { get; set; }
        public int ToSchoolIDThree { get; set; }

        public DateTime? TransferDate { get; set; }
        public string? RequestorComment  { get; set; }

        public string? FilePath { get; set; }
    }
}

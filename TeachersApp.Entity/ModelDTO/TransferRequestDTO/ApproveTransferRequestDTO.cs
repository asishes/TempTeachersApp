using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersApp.Entity.ModelDTO.TransferRequestDTO
{
    public class ApproveTransferRequestDTO
    {
        public int ToSchoolIDApproved { get; set; }
        public DateTime? TransferDate { get; set; }
        public string? ApproverComment { get; set; }

        public string? FilePath { get; set; }
    }
}

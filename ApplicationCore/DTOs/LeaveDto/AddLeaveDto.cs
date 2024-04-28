using ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTOs.LeaveDto
{
    public class AddLeaveDto
    {                                                                                                                   
        public LeaveType LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateTime? ResponseDate { get; set; }

        public DateTime RequestDate { get; set; }

        public ApprovalStatus ApprovalStatus { get; set; }

        public int Days { get; set; }
    }
}

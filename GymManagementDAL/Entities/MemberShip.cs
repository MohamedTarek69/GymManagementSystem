﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class MemberShip : BaseEntity
    {
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;
        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!;
        public DateTime EndDate { get; set; }

        //Read Only Property
        public string Status 
        { 
            get 
            {
                if (EndDate >= DateTime.Now)
                    return "Active";
                else
                    return "Expired";
            } 
        }
    }
}

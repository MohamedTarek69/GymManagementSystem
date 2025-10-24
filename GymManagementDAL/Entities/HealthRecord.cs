using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    // 1 - 1 RelationShip With Member [Shared PK]
    public class HealthRecord : BaseEntity
    {
        //Id => FK === PK[Id]
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string BloodType { get; set; } = null!;
        public string? Notes { get; set; }
    }
}

 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class HealthRecordViewModel
    {
        [Required(ErrorMessage = "Height Is Required")]
        [Range(0.1,300, ErrorMessage = "Height Must Be Between 0.1 And 300 cm")]
        public decimal Height { get; set; }

        [Required(ErrorMessage = "Weight Is Required")]
        [Range(0.1,500, ErrorMessage = "Weight Must Be Between 0.1 And 500 kg")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Blood Type Is Required")]
        [StringLength(3, ErrorMessage = "Blood Type Must Be 3 Characters Or Less")]
        [RegularExpression(@"^(A|B|AB|O)[+-]$", ErrorMessage = "Blood Type Must Be Valid")]
        public string BloodType { get; set; } = null!;

        public string? Notes { get; set; }
    }
}

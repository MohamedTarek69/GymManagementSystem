using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.PlanViewModels
{
    internal class UpdatePlanViewModel
    {
        public string PlanName { get; set; } = null!;

        [Required(ErrorMessage = "Description Is Required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Description Must Be Between 5 And 100 Characters")]
        public string Description { get; set; } = null!;
        
        [Required(ErrorMessage = "Duration Days Is Required")]
        [Range(1, 365, ErrorMessage = "Duration Days Must Be Between 1 And 365")]
        public int DurationDays { get; set; } 

        [Required(ErrorMessage = "Price Is Required")]
        [Range(0.1, 10000, ErrorMessage = "Price Must Be Between 0.1 And 10000")]
        public decimal Price { get; set; }

    }
}

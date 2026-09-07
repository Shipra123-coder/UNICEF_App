using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.ViewModels
{
    public class UserLevelModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User Level Name is required.")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
        [Display(Name = "User Level Name")]
        public string Name { get; set; }

        public DateTime? CreatedDate { get; set; }
    }

    public class UserLevelViewModel
    {
        public UserLevelModel FormModel { get; set; } = new UserLevelModel();
        public IEnumerable<UserLevelModel> UserLevelList { get; set; } = new List<UserLevelModel>();
    }
}

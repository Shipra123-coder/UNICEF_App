using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.WebsiteMaster
{
    public class NodalOfficerVM
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Name required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Designation required")]
        public string Designation { get; set; }
        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile must be 10 digits & Number Only")]
        public string Mobile { get; set; }
        [RegularExpression(@"^[0-9]{6,12}$", ErrorMessage = "Phone number must be 6 to 12 digits & Number Only")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter valid email address")]
        public string Email { get; set; }

        public int? DisplayOrder { get; set; }

        public bool? IsActive { get; set; }

        public List<NodalOfficerListVM>? OfficerList { get; set; }
    }
    public class NodalOfficerListVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Mobile { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}

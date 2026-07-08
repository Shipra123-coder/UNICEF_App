using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.GroupMaster
{
    public class GroupMO
    {
        public int Id { get; set; }
        public string? Guid { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only alphabets and spaces are allowed in Name")]
        public string Name { get; set; }

        public string? BtnSave { get; set; }
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only alphabets and spaces are allowed in Name")]
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }

        public List<GroupMO>? list { get; set; }
    }
}

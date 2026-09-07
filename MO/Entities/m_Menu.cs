using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Entities
{
    [Table("m_Menu")]
    public class m_Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("MenuId")]
        public long MenuId { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        public Guid? Guid { get; set; }

        [Required(ErrorMessage = "Menu Name is required.")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
        [Display(Name = "Menu Name (English)")]
        [Column("Name")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "Mangal Name cannot exceed 200 characters.")]
        [Display(Name = "Menu Name (Hindi / Mangal)")]
        [Column("MangalName")]
        public string? MangalName { get; set; }

        [Required(ErrorMessage = "Controller name is required (use # for Parent Dropdown).")]
        [StringLength(200, ErrorMessage = "Controller cannot exceed 200 characters.")]
        [Display(Name = "Controller")]
        [Column("Controller")]
        public string Controller { get; set; } = "#";

        [Required(ErrorMessage = "Action name is required (use # for Parent Dropdown).")]
        [StringLength(200, ErrorMessage = "Action cannot exceed 200 characters.")]
        [Display(Name = "Action Name")]
        [Column("ActionName")]
        public string ActionName { get; set; } = "#";

        [Display(Name = "Is SubMenu?")]
        [Column("IsSubMenu")]
        public bool IsSubMenu { get; set; } = false;

        [Display(Name = "Main / Parent Menu")]
        [Column("MainMenuId")]
        public long? MainMenuId { get; set; }

        [Display(Name = "Order Number")]
        [Range(1, int.MaxValue, ErrorMessage = "Order number must be positive.")]
        [Column("OrderNumber")]
        public int? OrderNumber { get; set; } = 1;

        [StringLength(200, ErrorMessage = "Icon class cannot exceed 200 characters.")]
        [Display(Name = "FontAwesome Icon")]
        [Column("Icon")]
        public string? Icon { get; set; } = "fa fa-list";

        [Display(Name = "Status")]
        [Column("IsActive")]
        public int? IsActive { get; set; } = 1; // 1 = Active, 0 = Inactive

        [Column("CreatedDate")]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        [Column("CreatedBy")]
        public string? CreatedBy { get; set; }

        [Column("UpdatedDate")]
        public DateTime? UpdatedDate { get; set; }

        [Column("UpdatedBy")]
        public string? UpdatedBy { get; set; }

        [ForeignKey("MainMenuId")]
        public virtual m_Menu? ParentMenu { get; set; }
    }

    public class MenuViewModel
    {
        public m_Menu FormModel { get; set; } = new m_Menu();
        public IEnumerable<m_Menu> MenuList { get; set; } = new List<m_Menu>();
        public IEnumerable<SelectListItem> MainMenuDropdownList { get; set; } = new List<SelectListItem>();
    }
}

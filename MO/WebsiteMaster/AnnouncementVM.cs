using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.WebsiteMaster
{
    public class Announcement
    {
        [ValidateNever]
        public int? Id { get; set; }
        [ValidateNever]
        public string? ExistingImage { get; set; }
        [ValidateNever]
        public IFormFile? ImageFile { get; set; }
        [Required(ErrorMessage = "Title required")]
        public string Title { get; set; }
        public bool IsNew { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Display order is required")]
        [Range(1, int.MaxValue,ErrorMessage = "Display order must be > 0")]
        public int? DisplayOrder { get; set; }
        [ValidateNever]
        public List<AnnouncementList>? AnnouncementList { get; set; }
    }
    public class mst_Announcement
    {
        public int? Id { get; set; }
        public string ImageBase64 { get; set; }
        public string Title { get; set; }
        public bool IsNew { get; set; }
        public bool IsActive { get; set; }
        public int? DisplayOrder { get; set; }
    }
    public class AnnouncementList
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public bool IsNew { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ImageBase64 { get; set; }
        public int? DisplayOrder { get; set; }
    }
    public class HomeAnnouncement
    {
        public string ImageBase64 { get; set; }
        public string Title { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsNew { get; set; }
    }
    public class ToggleAnnouncement
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
    }
}

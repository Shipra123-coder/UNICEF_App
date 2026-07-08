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
    public class SliderCreateVM
    {
        [ValidateNever]
        public int? Id { get; set; }
        public string? Guid { get; set; }
        [ValidateNever]
        public string? ExistingImage { get; set; }
        [ValidateNever]
        public IFormFile? ImageFile { get; set; }
        [Required(ErrorMessage = "Title is required")]

        public string Title { get; set; }
        [Required(ErrorMessage = "Display Order is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Display Order must be greater than 0")]
        public int? DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        [ValidateNever]
        public List<SliderListVM> SliderList { get; set; }
    }
    public class mst_Slider
    {
        public int? Id { get; set; }
        public string ImageBase64 { get; set; }
        public string Title { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
    public class SliderListVM
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ImageBase64 { get; set; }
    }
    public class HomePageVM
    {
        public List<HomePageSliderVM> Sliders { get; set; } = new();
        public List<AnnouncementVM> Announcements { get; set; } = new();
    }
    public class HomePageSliderVM
    {
        public string? ImageBase64 { get; set; }
        public string? Title { get; set; }
        public int? DisplayOrder { get; set; }
    }
    public class AnnouncementVM
    {
        public string? FileBase64 { get; set; }
        public string? Title { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsNew { get; set; }
    }

    public class ToggleSliderVM
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
    }

}

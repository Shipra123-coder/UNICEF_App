using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.WorkProgressModel
{
    public class WorkProgressModel
    {
        
        public int WorkId { get; set; } = 0;
     
        public int DistrictId { get; set; } = 0;    
        public string WorkStatus { get; set; }
        public int PhysicalProgress { get; set; }
        public string WorkItem { get; set; }
        public int NoOfWorks { get; set; }
        public int Target { get; set; }
        public int Achievement { get; set; }
        public string MajorAchievement { get; set; }
        public string Remarks { get; set; }
        public DateTime InspectionDate { get; set; }
        public string InspectionOfficer { get; set; }

        public IFormFile InspectionReport { get; set; }

        public IFormFile BeforePhoto { get; set; }
        public IFormFile DuringPhoto { get; set; }
        public IFormFile AfterPhoto { get; set; }
        public IFormFile VideoUpload { get; set; }
        public IFormFile ProgressReport { get; set; }
        public IFormFile AdminSanction { get; set; }
        public IFormFile TechnicalSanction { get; set; }
    }
}

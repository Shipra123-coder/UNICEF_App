using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Management
{
    public class ActivityMediaUploadDTO
    {
        public string ActivityGuid { get; set; }
        public string SubmissionMode { get; set; } // 'CoverPage' or 'InnerImage'
        public string Description { get; set; } // Shared description textarea field

        // Cover Page fields
        public string Heading { get; set; }
        public int OrderNumber { get; set; }
        public IFormFile CoverImage { get; set; }
        public IFormFile DescriptionPdf { get; set; }

        // Inner Page fields
        public long? ParentCoverId { get; set; }
        public string MediaType { get; set; }
        public IFormFile MediaFile { get; set; } // Handles dynamic input based on selection
    }
}

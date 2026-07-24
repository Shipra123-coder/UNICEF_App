using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.DashBoard
{
    public class BestPracticesModel
    {
        public long CoverPageId { get; set; }
        public string ActivityGuid { get; set; }
        public string HeadingTitle { get; set; }
        public int DisplayOrder { get; set; }
        public string CoverImageUrl { get; set; }
        public string DescriptionPdfUrl { get; set; }
        public string DescriptionNotes { get; set; }
        public long? SubActivityId { get; set; }
        public long? TaskId { get; set; }
        public long? GeoLocationId { get; set; }
        public int? SectorId { get; set; }
        public int? AgencyId { get; set; }
        public string SectorName { get; set; }

        public List<BestPracticesInnerMediaModel> MediaList { get; set; } = new();
    }
    public class BestPracticesInnerMediaModel
    {
        public long InnerMediaId { get; set; }
        public long CoverPageId { get; set; }
        public string MediaType { get; set; }
        public string MediaAssetUrl { get; set; }
        public string DescriptionNotes { get; set; }
    }
}

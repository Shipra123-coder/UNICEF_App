using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering; // <-- Ensure this namespace is used
using MO.Entities;

namespace MO.ViewModels
{
    public class SectorViewModel
    {
        public mst_Sector FormModel { get; set; } = new mst_Sector();
        public IEnumerable<mst_Sector> SectorList { get; set; } = new List<mst_Sector>();
        public IEnumerable<SelectListItem> PillarDropdownList { get; set; } = new List<SelectListItem>();
    }
}

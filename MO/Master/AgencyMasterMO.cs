using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Master
{
    public class AgencyMasterMO
    {
        public int Id { get; set; }
        public string AgencyName { get; set; }
        public string AgencyCode { get; set; }
        public string Category { get; set; }
        public int EstablishedYear { get; set; }
        public string Headquarters { get; set; }
        public string Country { get; set; }
        public string Websitelink { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }

        public List<AgencyMasterMO>? list { get; set; }
    }
}

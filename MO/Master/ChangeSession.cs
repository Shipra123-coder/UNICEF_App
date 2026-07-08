using MO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Master
{
    public class ChangeSession
    {
        public int? Id { get; set; }
        public List<FinancialYearDDL> FinancialYears { get; set; }
    }
}

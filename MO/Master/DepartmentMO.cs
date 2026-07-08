using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO.Master
{
    public class DepartmentMasterMO
    {
        public int Id { get; set; }

        public string DepartmentName { get; set; }

        public string ShortName { get; set; }

        public string DepartmentType { get; set; }

        public string HeadOfDepartment { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public List<DepartmentMasterMO>? list { get; set; }
    }
}
